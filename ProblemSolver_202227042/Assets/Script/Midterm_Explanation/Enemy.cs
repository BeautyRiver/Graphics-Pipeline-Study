using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform moveRange; // 이동 가능 거리(크기)
    public Camera camera; // 카메라
    public float moveSpeed; // 이동 속도
    public Vector3 moveDir; // 이동 방향
    public Transform min; // 이동 가능한 최소 위치
    public Transform max; // 이동 가능한 최대 위치

    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        // 이동 범위 설정
        //minBounds = moveRange.position - moveRange.localScale / 2;
        //maxBounds = moveRange.position + moveRange.localScale / 2;
        //StartCoroutine(CreateMoveDir());
    }

    void Update()
    {
        minBounds = moveRange.position - moveRange.localScale / 2; // 이동 가능한 최소 위치
        maxBounds = moveRange.position + moveRange.localScale / 2; // 이동 가능한 최대 위치
        min.transform.position = new Vector3(minBounds.x, minBounds.y, minBounds.z);
        max.transform.position = new Vector3(maxBounds.x, maxBounds.y, maxBounds.z);
        // 적 이동
        Vector3 newPosition = transform.position + moveDir * moveSpeed * Time.deltaTime; 

        // 이동할 새 위치가 범위 안에 있는지 확인
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x); 
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);
        transform.position = newPosition; 
        CheckDirection(); // 이동 방향 확인

        // 이동 방향을 바라보게 설정
        if (moveDir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }

        // 카메라 프러스텀에 Player가 있으면 Player를 향해 이동


    }

    IEnumerator CreateMoveDir() // 적의 이동 방향을 랜덤하게 설정
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }
    }
    void CheckDirection() // 이동 방향 확인
    {
        // 새 방향을 계산하고, 그 방향이 범위를 벗어나는지 확인
        Vector3 dectPos = transform.position + moveDir * 1; 

        // 만약 예상 위치가 범위를 벗어난다면, 새로운 방향을 선택
        if (dectPos.x < minBounds.x || dectPos.x > maxBounds.x || 
            dectPos.z < minBounds.z || dectPos.z > maxBounds.z)
            // 이동 가능한 최소 위치보다 작거나 이동 가능한 최대 위치보다 크다면
        {
            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(moveRange.position, moveRange.localScale);
        Gizmos.DrawRay(transform.position, moveDir * 2);


    }
       


}