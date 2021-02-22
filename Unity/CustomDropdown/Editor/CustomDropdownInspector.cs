using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor;

[CustomEditor(typeof(CustomDropdown), true)]
[CanEditMultipleObjects]
public class CustomDropdownInspector : SelectableEditor
{
	protected SerializedProperty m_blocker;
	protected SerializedProperty m_Template;
	protected SerializedProperty m_CaptionText;
	protected SerializedProperty m_CaptionImage;
	protected SerializedProperty m_ItemText;
	protected SerializedProperty m_ItemImage;
	protected SerializedProperty m_OnSelectionChanged;
	protected SerializedProperty m_Value;
	protected SerializedProperty m_Options;
	protected SerializedProperty m_AlphaFadeSpeed;
	protected override void OnEnable()
	{
		base.OnEnable();
		m_Template = serializedObject.FindProperty("m_Template");
		m_CaptionText = serializedObject.FindProperty("m_CaptionText");
		m_CaptionImage = serializedObject.FindProperty("m_CaptionImage");
		m_ItemText = serializedObject.FindProperty("m_ItemText");
		m_ItemImage = serializedObject.FindProperty("m_ItemImage");
		m_OnSelectionChanged = serializedObject.FindProperty("m_OnValueChanged");
		m_Value = serializedObject.FindProperty("m_Value");
		m_Options = serializedObject.FindProperty("m_Options");
		m_AlphaFadeSpeed = serializedObject.FindProperty("m_AlphaFadeSpeed");
		m_blocker = serializedObject.FindProperty("blocker");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();
		EditorGUILayout.PropertyField(m_Template);
		EditorGUILayout.PropertyField(m_CaptionText);
		EditorGUILayout.PropertyField(m_CaptionImage);
		EditorGUILayout.PropertyField(m_ItemText);
		EditorGUILayout.PropertyField(m_ItemImage);
		EditorGUILayout.PropertyField(m_Value);
		EditorGUILayout.PropertyField(m_AlphaFadeSpeed);
		EditorGUILayout.PropertyField(m_blocker);

		EditorGUILayout.PropertyField(m_Options);
		EditorGUILayout.PropertyField(m_OnSelectionChanged);
		serializedObject.ApplyModifiedProperties();
	}
}
