using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove_Exam : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
