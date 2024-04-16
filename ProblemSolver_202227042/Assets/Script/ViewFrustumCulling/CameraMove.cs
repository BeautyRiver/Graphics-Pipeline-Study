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
        if (Input.GetKey(KeyCode.Space))
        {
            y = 1;
        }       
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            y = -1;
        }
        else
        {
            y = 0;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(x,y,z) * speed * Time.deltaTime);
    }
}
