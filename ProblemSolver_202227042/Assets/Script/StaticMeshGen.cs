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

        if (GUILayout.Button("Mesh 생성~"))
        {
            script.GenerateMesh();
        }
    }
}

public class StaticMeshGen : MonoBehaviour
{
    public Material material;
    public float height;
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

        Vector3[] vertices = new Vector3[]
        {
            // 앞면
            new Vector3(0.0f, 0.0f, 0.0f), // 0
            new Vector3(1.0f, 2.5f, 0.0f), // 1
            new Vector3(2.0f, 0.0f, 0.0f), // 2
            // 뒷면
            new Vector3(0.0f, 0.0f, height), // 3
            new Vector3(1.0f, 2.5f, height), // 4
            new Vector3(2.0f, 0.0f, height), // 5
            /*-----------------------------------------------------*/
            // 앞면
            new Vector3(0.0f,1.8f,0.0f), // 6
            new Vector3(1.0f,-0.7f,0.0f), // 7
            new Vector3(2.0f,1.8f,0.0f), // 8
            // 뒷면
            new Vector3(0.0f,1.8f,height), // 9
            new Vector3(1.0f,-0.7f,height), // 10
            new Vector3(2.0f,1.8f,height), // 11
        };


        int[] triangles = new int[]
        {
            // 앞면
            0, 2, 1,
            // 뒷면
            3, 4, 5,
            // 옆면 1
            0, 1, 4,
            0, 4, 3,
            // 옆면 2
            1, 2, 5,
            1, 5, 4,
            // 옆면 3
            2, 0, 3,
            2, 3, 5,

            // 두번째

            // 앞면
            7,6,8,
            // 뒷면
            9,10,11,
            // 옆면 1
            6,7,10,
            6,10,9,
            // 옆면 2
            7,8,11,
            7,11,10,
            // 옆면 3
            8,6,9,
            8,9,11,

        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        if (material != null)
            mr.materials = new Material[] { material };

        mesh.RecalculateNormals(); // 메시의 노말을 재계산하여 올바른 라이팅을 보장

        mf.mesh = mesh;
    }
}
