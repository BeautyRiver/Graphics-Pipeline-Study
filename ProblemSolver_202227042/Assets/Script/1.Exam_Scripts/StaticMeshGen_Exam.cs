using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(StaticMeshGen_Exam))]
public class StaticMeshGenEditor1 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen_Exam script = (StaticMeshGen_Exam)target;

        if (GUILayout.Button("Mesh »ý¼º~"))
        {            
            script.CreatePoint();
            script.CreateLine();
            script.CreateTriangle();
        }
    }
}

public class StaticMeshGen_Exam : MonoBehaviour
{
    public Material material;
    public float height;

    public void CreatePoint()
    {
        GameObject pointObject = new GameObject("Point");
        Mesh mesh = new Mesh();
        pointObject.AddComponent<MeshFilter>().mesh = mesh;
        pointObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/PointSize"));

        mesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0) 
        };
        mesh.SetIndices(new int[] { 0 }, MeshTopology.Points, 0);
        pointObject.transform.position = new Vector3(-2, 0, 0);

    }

    public void CreateLine()
    {
        GameObject lineObject = new GameObject("Line");
        Mesh mesh = new Mesh();
        lineObject.AddComponent<MeshFilter>().mesh = mesh;
        lineObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        mesh.vertices = new Vector3[] 
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 1, 0) 
        };
        mesh.SetIndices(new int[] { 0, 1 }, MeshTopology.Lines, 0);
        lineObject.transform.position = new Vector3(0, 0, 0);
    }

    public void CreateTriangle()
    {
        GameObject triangleObject = new GameObject("Triangle");
        Mesh mesh = new Mesh();
        triangleObject.AddComponent<MeshFilter>().mesh = mesh;
        triangleObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        mesh.vertices = new Vector3[] 
        {
            new Vector3(0, 1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0) };
        mesh.triangles = new int[] {1,0,2 };
        triangleObject.transform.position = new Vector3(2, 0, 0);
    }
}
