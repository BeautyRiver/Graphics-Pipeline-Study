using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 moveDir;
    public float detectDistance;
    public Transform originalPosition;  // 원래 위치
    public Transform playerTransform;   // 플레이어의 Transform
    private Camera thisCamera;
    private FrustumPlanes frustumPlanes;
    private bool isLookingAround = false;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
        frustumPlanes = new FrustumPlanes(thisCamera);
        moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        if (PlayerInView())
        {
            // 플레이어를 발견하면 추적
            moveDir = (playerTransform.position - transform.position).normalized;
            if (!isLookingAround)
            {
                MoveEnemy();
            }
        }
        else if (!isLookingAround)
        {
            // 플레이어가 시야에서 벗어나면 주변 살피기
            StartCoroutine(LookAround());
        }
    }

    bool PlayerInView()
    {
        Bounds playerBounds = new Bounds(playerTransform.position, new Vector3(1, 1, 1));  // 플레이어의 경계 설정
        return frustumPlanes.IsInsideFrustum(playerBounds);
    }

    IEnumerator LookAround()
    {
        isLookingAround = true;
        Quaternion startRotation = transform.rotation;
        Quaternion leftRotation = Quaternion.Euler(0, -90, 0) * startRotation;
        Quaternion rightRotation = Quaternion.Euler(0, 180, 0) * startRotation;

        for (float t = 0; t < 1.5f; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, leftRotation, t / 1.5f);
            yield return null;
        }

        for (float t = 0; t < 3f; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(leftRotation, rightRotation, t / 3f);
            yield return null;
        }

        isLookingAround = false;
        moveDir = (originalPosition.position - transform.position).normalized;
    }

    void MoveEnemy()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 방향을 바라보게 설정
        if (moveDir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }
}
