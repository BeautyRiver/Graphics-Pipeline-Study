using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }
    }
}

public class StaticMeshGen : MonoBehaviour
{
    public Material material;
    private void Start()
    {
        GenerateMesh();
    }
    public void GenerateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (mf == null) mf = this.AddComponent<MeshFilter>();
        if (mr == null) mr = this.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // 가정: 별의 바닥면과 윗면을 이미 구성하는 정점이 정의되었음
        Vector3[] vertices = new Vector3[]
        {
            // 바닥면
            new Vector3(0.0f, 0.0f, 0.0f), // 0
            new Vector3(1.0f, 2.0f, 0.0f), // 1
            new Vector3(2.0f, 0.0f, 0.0f), // 2
            // 윗면
            new Vector3(0.0f, 0.0f, 1.0f), // 3
            new Vector3(1.0f, 2.0f, 1.0f), // 4
            new Vector3(2.0f, 0.0f, 1.0f), // 5
        };


        int[] triangles = new int[]
        {
            // 바닥면
            0, 2, 1,
            // 윗면
            3, 4, 5,
            // 옆면 1
            0, 1, 4,
            0, 4, 3,
            // 옆면 2
            1, 2, 5,
            1, 5, 4,
            // 옆면 3
            2, 0, 3,
            2, 3, 5
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        if (material != null)
            mr.materials = new Material[] { material };

        mesh.RecalculateNormals(); // 메시의 노말을 재계산하여 올바른 라이팅을 보장

        mf.mesh = mesh;
    }
}
