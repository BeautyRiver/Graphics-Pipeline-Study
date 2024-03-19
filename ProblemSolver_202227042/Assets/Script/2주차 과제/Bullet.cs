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
        if(isHit)
        {
            gameObject.SetActive(false);
            transform.position = initPos;
            colliders = null;
            isHit = false;

            player.Enqueue(gameObject);
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
