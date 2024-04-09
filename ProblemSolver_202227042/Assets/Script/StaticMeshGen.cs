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

        if (GUILayout.Button("Mesh ����~"))
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
            // �ո�
            new Vector3(0.0f, 0.0f, 0.0f), // 0
            new Vector3(1.0f, 2.5f, 0.0f), // 1
            new Vector3(2.0f, 0.0f, 0.0f), // 2
            // �޸�
            new Vector3(0.0f, 0.0f, height), // 3
            new Vector3(1.0f, 2.5f, height), // 4
            new Vector3(2.0f, 0.0f, height), // 5
            /*-----------------------------------------------------*/
            // �ո�
            new Vector3(0.0f,1.8f,0.0f), // 6
            new Vector3(1.0f,-0.7f,0.0f), // 7
            new Vector3(2.0f,1.8f,0.0f), // 8
            // �޸�
            new Vector3(0.0f,1.8f,height), // 9
            new Vector3(1.0f,-0.7f,height), // 10
            new Vector3(2.0f,1.8f,height), // 11
        };

        int[] triangles = new int[]
        {
            // �ո�
            0, 2, 1,
            // �޸�
            3, 4, 5,
            // ���� 1
            0, 1, 4,
            0, 4, 3,
            // ���� 2
            1, 2, 5,
            1, 5, 4,
            // ���� 3
            2, 0, 3,
            2, 3, 5,

            // �ι�°
            // �ո�
            7,6,8,
            // �޸�
            9,10,11,
            // ���� 1
            6,7,10,
            6,10,9,
            // ���� 2
            7,8,11,
            7,11,10,
            // ���� 3
            8,6,9,
            8,9,11,
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // ���� ���
        // ������ ���� ���͸� ������ �迭�� �ʱ�ȭ
        Vector3[] normals = new Vector3[vertices.Length];

        // ��� ������ ���� ���� ���͸� 0���� �ʱ�ȭ
        // �̴� �� ������ ���� ���͸� �ջ��ϱ� ���� �غ� �ܰ�
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.zero;
        }

        // �޽��� �����ϴ� �� �ﰢ���� ���� �ݺ�
        // triangles �迭�� �ﰢ���� �����ϴ� ������ �ε����� 3���� �׷�ȭ
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // �ﰢ���� �����ϴ� �� ������ �ε����� ������
            int indexA = triangles[i];
            int indexB = triangles[i + 1];
            int indexC = triangles[i + 2];

            // �ε����� ����Ͽ� ���� ������ ��ġ�� ������
            Vector3 vertexA = vertices[indexA];
            Vector3 vertexB = vertices[indexB];
            Vector3 vertexC = vertices[indexC];

            // �ﰢ���� �� ���� AB�� AC�� �����
            Vector3 ab = vertexB - vertexA;
            Vector3 ac = vertexC - vertexA;

            // AB�� AC�� ������ ����Ͽ� �ﰢ���� ���� ���͸� ����
            // ���� ����� �� ���Ϳ� ��� ������ �����̸�, �ﰢ���� ������ ��Ÿ��
            // Normalize�� ���� ���� ������ ���̸� 1�� ����
            Vector3 normal = Vector3.Cross(ab, ac).normalized;

            // �� �������� �ﰢ���� ���� ���͸� �ջ�
            // �̴� �� ������ ���� �ﰢ���� ���� ��� ��� ���� ���͸� ���ϱ� ����
            normals[indexA] += normal;
            normals[indexB] += normal;
            normals[indexC] += normal;
        }

        // ��� ������ ���� ���� ���͸� ����ȭ
        // �̴� �ջ�� ���� ������ ���̸� 1�� ����� ��Ȯ�� ���⸸�� ������ ��
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        // ���� ���� ���͸� �޽��� �Ҵ�
        // ���� �޽��� �� �������� ��Ȯ�� ������ ����Ű�� ���� ���͸� ������ �Ǹ�,
        // �̸� ���� �����ð� ���̵��� �ùٸ��� ����
        mesh.normals = normals;


        // Material �Ҵ�
        if (material != null)
            mr.materials = new Material[] { material };

        //mesh.RecalculateNormals(); // �޽��� �븻�� �����Ͽ� �ùٸ� �������� ����

        mf.mesh = mesh;
    }
}
