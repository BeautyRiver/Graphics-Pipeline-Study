using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("플레이어 추격 옵션")]
    public Camera camera;
    public Renderer playerRenderer;
    public bool isChase = false;
    public bool wasChasing = false;  // 처음에는 추격 상태가 아니라고 가정
    public bool isReturnPos = false;
    public  bool isLookAround = false; // 주변을 둘러보는 중인지 여부

    [Header("이동 제한 & 범위")]
    public Transform moveRange;
    public float moveSpeed;
    public Vector3 moveDir;
    public float dectRange;
    private Vector3 newPosition;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private Vector3 originalPosition;

    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        originalPosition = moveRange.position;
        minBounds = moveRange.position - moveRange.localScale / 2;
        maxBounds = moveRange.position + moveRange.localScale / 2;
        StartCoroutine(CreateMoveDir());
    }

    void Update()
    {
        Move();
        if (isChase == false && isReturnPos == false && isLookAround == false) // 추격 중이 아닐 때
            CheckDirection();       

        FindPlayer();

    }

    
    void Move()
    {
        if (isChase)
        {
            // 플레이어를 향해 이동
            moveDir = (playerRenderer.transform.position - transform.position).normalized;
        }
        else if (isReturnPos)
        {
            // 원래 위치로 이동
            moveDir = (originalPosition - transform.position).normalized;
            if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
            {
                isReturnPos = false;
                if (!IsInvoking(nameof(CreateMoveDir))) // 중복 실행 방지
                    StartCoroutine(CreateMoveDir());
            }
        }
        else
        {
            // 이동 제한 범위 설정
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);
        }
        newPosition = transform.position + moveDir * moveSpeed * Time.deltaTime;
        transform.position = newPosition;


        // 이동 방향을 바라보게 설정
        if (moveDir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }

  

    void FindPlayer()
    {
        FrustumPlanes frustum = new FrustumPlanes(camera);
        if (frustum.IsInsideFrustum(playerRenderer.bounds))
        {
            if (!isChase)
            {
                StopAllCoroutines();
                isChase = true;
                isReturnPos = false;
                wasChasing = true;
                
            }
        }
        else
        {
            if (wasChasing) // 최초에 카메라에 감지가 안되므로 추격 상태에서 벗어나서 wasChasing으로 감지
            {
                StopCoroutine(CreateMoveDir());
                if (!IsInvoking(nameof(LookAround))) // 중복 실행 방지
                    StartCoroutine(LookAround());
                wasChasing = false;
                isChase = false;
                isLookAround = true;
            }
            
        }
    }


    // 플레이어를 놓쳤을 때 주변을 둘러봄
    IEnumerator LookAround() 
    {
        moveDir = Vector3.zero; // 이동 방향 초기화
        Quaternion originalRotation = transform.rotation; // 원래 방향 저장
        Quaternion leftRotation = Quaternion.Euler(0, -90, 0) * originalRotation; // 좌측으로 90도 회전
        Quaternion rightRotation = Quaternion.Euler(0, -180, 0) * leftRotation;  // 우측으로 180도 회전

        // 좌측으로 90도 회전
        for (float t = 0; t < 1; t += Time.deltaTime / 3)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, leftRotation, t);
            yield return null;
        }
        // 우측으로 180도 회전
        for (float t = 0; t < 1; t += Time.deltaTime / 3)
        {
            transform.rotation = Quaternion.Lerp(leftRotation, rightRotation, t);
            yield return null;
        }
        isReturnPos = true;
        isLookAround = false;
    }


    // 일정 시간마다 이동 방향을 바꿈
    IEnumerator CreateMoveDir()
    {
        while (true)
        {
            if (!isChase && !isReturnPos && !isLookAround) // 추격 또는 복귀 중이 아닐 때에만 방향 변경
            {
                moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
            }
            else
            {
                yield return null; // 추격 또는 복귀 중일 때는 대기
            }
        }
    }

    // 이동 방향이 범위를 벗어나면 방향을 바꿈
    void CheckDirection() 
    {
        Vector3 detectPos = transform.position + moveDir * dectRange;
        if (detectPos.x < minBounds.x || detectPos.x > maxBounds.x || detectPos.z < minBounds.z || detectPos.z > maxBounds.z)
        {
            moveDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(moveRange.position, moveRange.localScale);
    }
}
