using UnityEngine;

public class EnemyMove_Exam : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private float currentSpeed;
    public LayerMask wallLayer;
    private void Update()
    {
        currentSpeed = speed;
        // Ÿ���� ���� �����̱�
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);
        
    }
}
