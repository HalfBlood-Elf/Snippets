using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class GpuGroupInstancer : MonoBehaviour
{
	public MeshRendererWithRegisteredMeshObject[] registeredMeshRenderers = new MeshRendererWithRegisteredMeshObject[0];

	public MeshObject[] registeredMeshes = new MeshObject[0];

	private void Awake()
	{
		foreach (var registeredMesh in registeredMeshes)
		{
			var matchingMeshRenderers = registeredMeshRenderers.Where(x => x.mesh == registeredMesh.mesh);
			var draw_matrixes = new List<Matrix4x4>();
			foreach (var item in matchingMeshRenderers)
			{
				if (item.meshRenderer == null)
				{
					Debug.LogError($"{name} GpuGroupInstancer have null renderer", gameObject);
					continue;
				}

				if (!item.meshRenderer.gameObject.activeInHierarchy) continue;
				item.meshRenderer.transform.parent = null;
				draw_matrixes.Add(Matrix4x4.TRS(item.meshRenderer.transform.position, item.meshRenderer.transform.rotation, item.meshRenderer.transform.localScale));
				Destroy(item.meshRenderer.gameObject);
			}
			//Debug.Log($"For Mesh {registeredMesh.mesh.name} added {draw_matrixes.Count} matrixes");
			registeredMesh.draw_matrixes = draw_matrixes.ToArray();
		}
	}


	private void Update()
	{

		foreach (var registeredMesh in registeredMeshes)
		{
			for (int i = 0; i < registeredMesh.materials.Length; i++)
			{
				try
				{
					Graphics.DrawMeshInstanced(registeredMesh.mesh, i, registeredMesh.materials[i], registeredMesh.draw_matrixes, registeredMesh.draw_matrixes.Length);
				}
				catch (System.InvalidOperationException)
				{
					Debug.LogError($"Gpu instancing is not enabled for {registeredMesh.materials[i].name} material");
					continue;

				}
			}

		}
	}


	[System.Serializable]
	public class MeshRendererWithRegisteredMeshObject
	{
		public MeshRenderer meshRenderer;
		public Mesh mesh;

		//private Vector3 localPos;
		//private Quaternion localRot;
		//private Vector3 localScale;

		//public void SetLocalTransform(Transform transform)
		//{
		//	localPos = transform.position;
		//}

	}


	[System.Serializable]
	public class MeshObject
	{
		public Material[] materials;
		public Mesh mesh;
		[HideInInspector] public Matrix4x4[] draw_matrixes;
	}

}

#if UNITY_EDITOR

[CustomEditor(typeof(GpuGroupInstancer)), CanEditMultipleObjects]
public class GpuGroupInstancerEditor : Editor
{
	private GpuGroupInstancer script;
	private bool componentChanged;
	private void OnEnable()
	{
		script = (GpuGroupInstancer)target;

	}

	public override void OnInspectorGUI()
	{
		componentChanged = false;
		object[] objs = DropZone("Drag & Drop Renderers Or Parents Here To Add Objects", 300, 50);
		AddDroppedObjects(objs, false);

		serializedObject.ApplyModifiedProperties();
		if (componentChanged) EditorUtility.SetDirty(target);

		base.OnInspectorGUI();

	}

	public object[] DropZone(string title, int w, int h)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box(title, GUILayout.Width(w), GUILayout.Height(h));
		Rect dropRect = GUILayoutUtility.GetLastRect();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EventType eventType = Event.current.type;
		bool isAccepted = false;

		if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
		{
			if (dropRect.Contains(Event.current.mousePosition))
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				if (eventType == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();
					isAccepted = true;
					//Debug.Log("Consuming drop event in inspector. " + Event.current.mousePosition + " rect" + dropRect);
					Event.current.Use();
				}
			}
		}

		return isAccepted ? DragAndDrop.objectReferences : null;
	}

	public void AddDroppedObjects(object[] objs, bool includeInactive)
	{
		if (objs != null)
		{
			var renderersToAdd = new HashSet<GpuGroupInstancer.MeshRendererWithRegisteredMeshObject>();
			var currentTargets = script.registeredMeshRenderers;
			foreach (var currentTarget in currentTargets)
			{
				renderersToAdd.Add(currentTarget);
			}

			var meshesToAdd = new HashSet<GpuGroupInstancer.MeshObject>();
			var currentRegisteredMeshes = script.registeredMeshes;
			foreach (var currentTarget in currentRegisteredMeshes)
			{
				meshesToAdd.Add(currentTarget);
			}


			var startNumbOfObjs = renderersToAdd.Count;
			for (int i = 0; i < objs.Length; i++)
			{
				object obj = objs[i];
				if (obj is GameObject)
				{
					MeshRenderer[] meshRenderers = ((GameObject)obj).GetComponentsInChildren<MeshRenderer>(includeInactive);
					for (int j = 0; j < meshRenderers.Length; j++)
					{
						if (includeInactive || meshRenderers[j].enabled)
						{
							var meshRend = new GpuGroupInstancer.MeshRendererWithRegisteredMeshObject
							{
								meshRenderer = meshRenderers[j],
							};
							var mesh = meshRend.meshRenderer.GetComponent<MeshFilter>()?.sharedMesh;
							if (meshRend.meshRenderer.GetComponent<TMPro.TextMeshPro>() != null) continue;
							var registeredMesh = meshesToAdd.FirstOrDefault(x => x.mesh == mesh);
							if(registeredMesh == null)
							{
								registeredMesh = new GpuGroupInstancer.MeshObject()
								{
									mesh = mesh,
									materials = meshRenderers[j].sharedMaterials
								};
								CheckMaterialsEnabledGpuInstancing(registeredMesh.materials, true);
								meshesToAdd.Add(registeredMesh);
							}
							meshRend.mesh = mesh;
							renderersToAdd.Add(meshRend);
						}

					}
				}
			}

			if (startNumbOfObjs != renderersToAdd.Count)
			{
				Undo.RecordObject(target, "Set Field Value");
				script.registeredMeshRenderers = renderersToAdd.ToArray();
				script.registeredMeshes = meshesToAdd.ToArray();
				componentChanged = true;
			}

		}
	}

	private void CheckMaterialsEnabledGpuInstancing(IEnumerable<Material> materials, bool enableDefault)
	{
		foreach (var mat in materials)
		{
			if(enableDefault) mat.enableInstancing = true;
			else if(!enableDefault && !mat.enableInstancing)
			{
				Debug.LogError($"Gpu instancing is not enabled for {mat.name} material");
			}
		}
	}
}

#endif
