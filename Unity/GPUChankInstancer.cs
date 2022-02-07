using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUChankInstancer : MonoBehaviour
{
    private int ChunkObjectsCount;
    public float TreesCompensationAngle;

    public Material draw_material;
    public Mesh draw_mesh;
    public Matrix4x4[] draw_matrices;


    public void Setup()
    {

        if (transform.childCount < 1024)
        {
            draw_matrices = new Matrix4x4[transform.childCount];

        }
        else
        {
            draw_matrices = new Matrix4x4[1023];
        }

        for (int i = 0; i < draw_matrices.Length; i++)
        {
            var child = transform.GetChild(i);
            // Build matrix.
            Vector3 position = new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, transform.GetChild(i).position.z);
            Quaternion rotation = Quaternion.Euler(transform.GetChild(i).eulerAngles.x + TreesCompensationAngle, transform.GetChild(i).eulerAngles.y, transform.GetChild(i).eulerAngles.z);
            Vector3 scale = new Vector3(transform.GetChild(i).localScale.x, transform.GetChild(i).localScale.y, transform.GetChild(i).localScale.z);
            // draw_matrices[i] = new Matrix4x4();
            draw_matrices[i] = Matrix4x4.TRS(position, rotation, scale);
        }
    }


    void Start()
    {
        ChunkObjectsCount = transform.childCount;
        Setup();

        Transform[] chlds = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            chlds[i] = transform.GetChild(i);
        }
        transform.localPosition = FindThePivot(chlds);
        // foreach (Transform chld in transform)
        // {
        //     chld.gameObject.SetActive(false);
        // }

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        // Draw a bunch of meshes each frame.
        Graphics.DrawMeshInstanced(draw_mesh, 0, draw_material, draw_matrices, draw_matrices.Length);

        // Debug.Log(transform.position);
        // Debug.Log("LOCAL: " + transform.localPosition);
    }

    Vector3 FindThePivot(Transform[] trans)
    {
        if (trans == null || trans.Length == 0)
            return Vector3.zero;
        if (trans.Length == 1)
            return trans[0].position;

        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        float minZ = Mathf.Infinity;

        float maxX = -Mathf.Infinity;
        float maxY = -Mathf.Infinity;
        float maxZ = -Mathf.Infinity;

        foreach (Transform tr in trans)
        {
            if (tr.position.x < minX)
                minX = tr.position.x;
            if (tr.position.y < minY)
                minY = tr.position.y;
            if (tr.position.z < minZ)
                minZ = tr.position.z;

            if (tr.position.x > maxX)
                maxX = tr.position.x;
            if (tr.position.y > maxY)
                maxY = tr.position.y;
            if (tr.position.z > maxZ)
                maxZ = tr.position.z;
        }

        return new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, (minZ + maxZ) / 2.0f);
    }
}
