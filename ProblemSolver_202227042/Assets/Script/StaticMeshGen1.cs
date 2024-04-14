using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(StaticMeshGen1))]
public class StaticMeshGenEditor1 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen1 script = (StaticMeshGen1)target;

        if (GUILayout.Button("Mesh ����~"))
        {
            script.GenerateMesh();
        }
    }
}

public class StaticMeshGen1 : MonoBehaviour
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
            // ������ �׸���

            // ��� �ﰢ��            
            // �ո�
            new Vector3(0.0f, 0.0f, 0.0f), // 0 ����            
            new Vector3(-2.0f, -6.0f, 0.0f), // 1
            new Vector3(2.0f, -6.0f, 0.0f), // 2

            // ���� �ﰢ��
            new Vector3(-3.3f, -2.2f, 0.0f), // 3          

            // ������ �ﰢ��
            new Vector3(3.3f, -2.2f, 0.0f), // 4

            // �޸�
            // ��� �ﰢ��            
            new Vector3(0.0f, 0.0f, height), // 5 ����            
            new Vector3(-2.0f, -6.0f, height), // 6
            new Vector3(2.0f, -6.0f, height), // 7

            // ���� �ﰢ��
            new Vector3(-3.3f, -2.2f, height), // 8          

            // ������ �ﰢ��
            new Vector3(3.3f, -2.2f, height), // 9

            // �� �ٸ� �׸���
            // �ո�
            new Vector3(4.06f,2.09f,0f), // 10
            new Vector3(6.47f,-5.28f,0f), // 11
            new Vector3(0f,-10.00f,0f), // 12
            new Vector3(-6.47f,-5.28f,0f), // 13
            new Vector3(-4.06f, 2.09f, 0f), // 14 

            // �޸�
            new Vector3(4.06f,2.09f, height), // 15
            new Vector3(6.47f,-5.28f, height), // 16
            new Vector3(0f,-10.00f, height), // 17
            new Vector3(-6.47f,-5.28f,height), // 18
            new Vector3(-4.06f, 2.09f, height), // 19 

            // ����

                                    
        };

        // �ﰢ�� �ε��� ����
        int[] triangles = new int[]
        {
            // ������ ���
            // ������ �ո�
            0,2,1,
            3,0,1,
            0,4,2,
            
            // ������ �޸�
            5,6,7,
            5,8,6,
            7,9,5,

            // ���� ����
            3,5,0,
            8,5,3,
            3,1,6,
            8,3,6,
            // ���� ������
            0,5,9,
            0,9,4,
            2,4,7,
            7,4,9,

            // �Ʒ���
            7,1,2,
            6,1,7,

            //----------------------------------------------------------------

            // �� �׸���
            // �ո�
            4,0,10,
            2,4,11,
            1,2,12,
            3,1,13,
            0,3,14,

            // �޸�
            5,9,15,
            9,7,16,
            6,17,7,
            8,18,6,
            19,8,5,
            
            // ����
            0,5,15,
            0,15,10,

            4,10,15,
            4,15,9,

            16,4,9,
            16,11,4,

            16,2,11,
            16,7,2,

            12,2,7,
            12,7,17,

            12,6,1,
            12,17,6,
            
            6,13,1,
            6,18,13,
            
            18,3,13,
            18,8,3,

            8,14,3,
            8,19,14,

            19,0,14,
            19,5,0,
        };

        // ���� ���� ���
        Vector3[] normals = new Vector3[vertices.Length];

        // �� �ﰢ���� ���� ���� ���
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            normals[triangles[i]] += normal;
            normals[triangles[i + 1]] += normal;
            normals[triangles[i + 2]] += normal;
        }

        // ������ ����ȭ
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        // Mesh ��ü�� ������, �ﰢ��, ���� �Ҵ�
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // Material �Ҵ�
        if (material != null)
            mr.materials = new Material[] { material };


        mf.mesh = mesh;
    }
}
