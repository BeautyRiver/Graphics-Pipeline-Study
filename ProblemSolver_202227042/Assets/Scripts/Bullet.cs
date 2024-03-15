using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PlayerShot player;
    Vector3 initPos;
    private void Awake()
    {
         initPos = transform.position;
         player = FindObjectOfType<PlayerShot>();
    }
    private void Update()
    {
        transform.Translate(Vector3.right * 5.0f * Time.deltaTime);

        if(transform.position.x > 8)
        {
            gameObject.SetActive(false);
            transform.position = initPos;
            player.Enqueue(gameObject);
            Debug.Log("Enqueue bullet: " + gameObject.name);
        }
    }


}
