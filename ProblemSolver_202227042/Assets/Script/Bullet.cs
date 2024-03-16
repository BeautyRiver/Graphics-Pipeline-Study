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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            transform.position = initPos;
            player.Enqueue(gameObject);
            Debug.Log("Enqueue bullet: " + gameObject.name);
        }
    }


}
