using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    public Transform moveRange; // 이동 가능 거리(크기)
    public float moveSpeed;
    public Vector3 moveDir;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        // 이동 범위 설정
        minBounds = moveRange.position - moveRange.localScale / 2;
        maxBounds = moveRange.position + moveRange.localScale / 2;
        StartCoroutine(CreateMoveDir());
    }

    void Update()
    {
        // 적 이동
        Vector3 newPosition = transform.position + moveDir * moveSpeed * Time.deltaTime;

        // 이동할 새 위치가 범위 안에 있는지 확인
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);
        transform.position = newPosition;
        CheckDirection();
        // 이동 방향을 바라보게 설정
        if (moveDir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }

    IEnumerator CreateMoveDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            
        }
    }
    void CheckDirection()
    {
        // 새 방향을 계산하고, 그 방향이 범위를 벗어나는지 확인
        Vector3 testPosition = transform.position + moveDir * 10; // 예상 이동 거리

        // 만약 예상 위치가 범위를 벗어난다면, 새로운 방향을 선택
        if (testPosition.x < minBounds.x || testPosition.x > maxBounds.x ||
            testPosition.z < minBounds.z || testPosition.z > maxBounds.z)
        {
            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(moveRange.position,moveRange.localScale);
    }


}
