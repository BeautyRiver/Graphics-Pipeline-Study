using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 moveDir;
    public float detectDistance;
    public Transform originalPosition;  // ���� ��ġ
    public Transform playerTransform;   // �÷��̾��� Transform
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
            // �÷��̾ �߰��ϸ� ����
            moveDir = (playerTransform.position - transform.position).normalized;
            if (!isLookingAround)
            {
                MoveEnemy();
            }
        }
        else if (!isLookingAround)
        {
            // �÷��̾ �þ߿��� ����� �ֺ� ���Ǳ�
            StartCoroutine(LookAround());
        }
    }

    bool PlayerInView()
    {
        Bounds playerBounds = new Bounds(playerTransform.position, new Vector3(1, 1, 1));  // �÷��̾��� ��� ����
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

        // ������ �ٶ󺸰� ����
        if (moveDir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }
}
