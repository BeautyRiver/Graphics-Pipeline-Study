using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    public Transform moveRange; // �̵� ���� �Ÿ�(ũ��)
    public float moveSpeed;
    public Vector3 moveDir;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        // �̵� ���� ����
        minBounds = moveRange.position - moveRange.localScale / 2;
        maxBounds = moveRange.position + moveRange.localScale / 2;
        StartCoroutine(CreateMoveDir());
    }

    void Update()
    {
        // �� �̵�
        Vector3 newPosition = transform.position + moveDir * moveSpeed * Time.deltaTime;

        // �̵��� �� ��ġ�� ���� �ȿ� �ִ��� Ȯ��
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);
        transform.position = newPosition;
        CheckDirection();
        // �̵� ������ �ٶ󺸰� ����
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
        // �� ������ ����ϰ�, �� ������ ������ ������� Ȯ��
        Vector3 testPosition = transform.position + moveDir * 10; // ���� �̵� �Ÿ�

        // ���� ���� ��ġ�� ������ ����ٸ�, ���ο� ������ ����
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
