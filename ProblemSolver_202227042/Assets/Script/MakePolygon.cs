using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakePolygon : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // ���� ����
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 1, 0), // ���� 0
            new Vector3(0, 0, 0), // ���� 1
            new Vector3(2, 1, 0)  // ���� 2
        };

        // �� ����
        int[] triangles = new int[]
        {
            1, 0, 2 // ù ��° �ﰢ��
        };

        // �޽��� ������ �� �Ҵ�
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        // ��� ���� (�ɼ�)
        mesh.RecalculateNormals();                
    }


}
