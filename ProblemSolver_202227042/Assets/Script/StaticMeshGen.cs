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

        // 법선 계산
        // 정점별 법선 벡터를 저장할 배열을 초기화
        Vector3[] normals = new Vector3[vertices.Length];

        // 모든 정점에 대해 법선 벡터를 0으로 초기화
        // 이는 각 정점의 법선 벡터를 합산하기 위한 준비 단계
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.zero;
        }

        // 메쉬를 구성하는 각 삼각형에 대해 반복
        // triangles 배열은 삼각형을 구성하는 정점의 인덱스를 3개씩 그룹화
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // 삼각형을 구성하는 각 정점의 인덱스를 가져옴
            int indexA = triangles[i];
            int indexB = triangles[i + 1];
            int indexC = triangles[i + 2];

            // 인덱스를 사용하여 실제 정점의 위치를 가져옴
            Vector3 vertexA = vertices[indexA];
            Vector3 vertexB = vertices[indexB];
            Vector3 vertexC = vertices[indexC];

            // 삼각형의 두 벡터 AB와 AC를 계산함
            Vector3 ab = vertexB - vertexA;
            Vector3 ac = vertexC - vertexA;

            // AB와 AC의 외적을 계산하여 삼각형의 법선 벡터를 구함
            // 외적 결과는 두 벡터에 모두 수직인 벡터이며, 삼각형의 방향을 나타냄
            // Normalize를 통해 법선 벡터의 길이를 1로 만듬
            Vector3 normal = Vector3.Cross(ab, ac).normalized;

            // 각 정점별로 삼각형의 법선 벡터를 합산
            // 이는 한 정점이 여러 삼각형에 속할 경우 평균 법선 벡터를 구하기 위함
            normals[indexA] += normal;
            normals[indexB] += normal;
            normals[indexC] += normal;
        }

        // 모든 정점에 대해 법선 벡터를 정규화
        // 이는 합산된 법선 벡터의 길이를 1로 만들어 정확한 방향만을 가지게 함
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        // 계산된 법선 벡터를 메쉬에 할당
        // 이제 메쉬는 각 정점에서 정확한 방향을 가리키는 법선 벡터를 가지게 되며,
        // 이를 통해 라이팅과 쉐이딩이 올바르게 적용
        mesh.normals = normals;


        // Material 할당
        if (material != null)
            mr.materials = new Material[] { material };

        //mesh.RecalculateNormals(); // 메시의 노말을 재계산하여 올바른 라이팅을 보장

        mf.mesh = mesh;
    }
}
