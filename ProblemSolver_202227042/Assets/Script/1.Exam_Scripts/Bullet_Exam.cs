using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet_Exam : MonoBehaviour
{
    public float speed;
    public Vector3 size; // �Ѿ��� AABB ũ��
    public Vector3 colliderPos; // �Ѿ� �ݶ��̴� ��ġ
    public LayerMask wallLayer;
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Collider[] col = Physics.OverlapBox(transform.position + colliderPos, size, Quaternion.identity, wallLayer);
        if (col.Length > 0)
        {
            PlayerShot_Exam.EnqueueBullet?.Invoke(gameObject);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + colliderPos, size);

    }
}
