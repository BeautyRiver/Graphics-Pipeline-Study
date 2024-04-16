using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerShot player;
    private BoxCollider boxCollider;
    private Vector3 initPos;
    private Vector3 bulletPos;
    private Vector3 checkBoxSize;
    private bool isHit;
    [SerializeField]
    private LayerMask enemyLayer;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        initPos = transform.position;
        player = FindObjectOfType<PlayerShot>();
        checkBoxSize = transform.localScale;
    }
    private void Update()
    {        
        transform.Translate(Vector3.right * 5.0f * Time.deltaTime);
        Bounds bounds = boxCollider.bounds;
        bulletPos = new Vector3(bounds.center.x, bounds.center.y, bounds.center.z);
        Collider[] colliders = Physics.OverlapBox(bulletPos, checkBoxSize, Quaternion.identity, enemyLayer);                
        if(colliders.Length > 0 )
        {
            isHit = true;
        }

        if (isHit)
        {
            gameObject.SetActive(false);
            transform.position = initPos;
            colliders = null;
            isHit = false;

            // Queue 일때
            if(player.selectDataStruct == PlayerShot.DataStructure.Queue)
                player.queue.Enqueue(gameObject);

            // Stack 일때
            else if (player.selectDataStruct == PlayerShot.DataStructure.Stack)
                player.stack.queue1.Enqueue(gameObject); // 어차피 역순으로 queue를 해놓았기 때문에 다시 queue로 쌓고 dequeue하면 똑같음

            Debug.Log("Enqueue bullet: " + gameObject.name);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            transform.position = initPos;
            player.Enqueue(gameObject);
            Debug.Log("Enqueue bullet: " + gameObject.name);
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(bulletPos, checkBoxSize);
    }


}
