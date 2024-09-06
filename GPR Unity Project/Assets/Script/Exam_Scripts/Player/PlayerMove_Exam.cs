using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


#if UNITY_EDITOR
[ExecuteInEditMode]
#endif

public class PlayerMove_Exam : MonoBehaviour
{
    public enum CollisionType
    {
        AABB,
        Distance,
    };
    public CollisionType collisionType;
    private Rigidbody rb;
    private PlayerShot_Exam ps;
    public float speed = 5.0f;
    public float rotSpeed = 3.0f;
    private Vector3 dir;

    // AABB
    public Transform enemy; // ���� ��ġ
    public Vector3 playerColliderPos;
    public Vector3 enemySize; // ���� ũ�� AABB �ݰ�
    public Vector3 enemyColliderPos; // ���� �ݶ��̴� ��ġ;

    // Distance
    public float radius; // ������ ũ��
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerShot_Exam>();
    }

    private void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        dir.Normalize();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ps.Shot();
        }

        if (collisionType == CollisionType.AABB)
        {
            // AABB����
            Vector3 playerMin = transform.position - transform.localScale / 2;
            Vector3 playerMax = transform.position + transform.localScale / 2;
            Vector3 enemyMin = enemy.position - enemySize / 2;
            Vector3 enemyMax = enemy.position+ enemySize / 2;
            
            if (IsAABBColliding(playerMin, playerMax, enemyMin, enemyMax))
            {
                Debug.Log("AABB �浹!");
            }
        }
        else if (collisionType == CollisionType.Distance)
        {
            if (IsCircleColliding(transform.position + enemyColliderPos, transform.localScale.x, enemy.position, radius))
            {
                Debug.Log("Circle �浹!");
            }
        }        
    }
    // AABB
    private bool IsAABBColliding(Vector3 playerMin, Vector3 playerMax, Vector3 enemyMin, Vector3 enemyMax)
    {
        return (playerMin.x <= enemyMax.x && playerMax.x >= enemyMin.x) &&
               (playerMin.y <= enemyMax.y && playerMax.y >= enemyMin.y) &&
               (playerMin.z <= enemyMax.z && playerMax.z >= enemyMin.z);
    }

    // Circle Collision (Distance)
    bool IsCircleColliding(Vector3 playerCenter, float playerRadius, Vector3 enemyCenter, float enemyRadius)
    {
        float distance = Vector3.Distance(playerCenter, enemyCenter);
        return distance <= (playerRadius + enemyRadius);
    }

    #region �̵�
    private void FixedUpdate()
    {
        if (dir != Vector3.zero)
        {            
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }
            //transform.LookAt(Vector3.Lerp(transform.position, transform.position + dir, rotSpeed));
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.fixedDeltaTime);
        }
        rb.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
    }
    #endregion

    #region �����
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (collisionType == CollisionType.AABB)
        {
            Gizmos.DrawWireCube((enemy.position + enemyColliderPos), enemySize);
            Gizmos.DrawWireCube((transform.position + playerColliderPos), transform.localScale);
        }
        else if (collisionType == CollisionType.Distance)
        {
            Gizmos.DrawWireSphere(enemy.position + enemyColliderPos, radius);
            Gizmos.DrawWireSphere(transform.position + enemyColliderPos, transform.localScale.x);

        }
    }
#endif
    #endregion
}
