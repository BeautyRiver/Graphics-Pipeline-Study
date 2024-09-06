using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    float x;
    float z;
    float y;
    public float speed;
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
      
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(x,z,0) * speed * Time.deltaTime);
    }
}
