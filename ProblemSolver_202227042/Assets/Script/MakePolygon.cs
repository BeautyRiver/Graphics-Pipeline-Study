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

        // 정점 정의
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 1, 0), // 정점 0
            new Vector3(0, 0, 0), // 정점 1
            new Vector3(2, 1, 0)  // 정점 2
        };

        // 면 정의
        int[] triangles = new int[]
        {
            1, 0, 2 // 첫 번째 삼각형
        };

        // 메쉬에 정점과 면 할당
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        // 노멀 재계산 (옵션)
        mesh.RecalculateNormals();                
    }


}
