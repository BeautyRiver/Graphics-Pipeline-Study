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
        // 타겟을 향해 움직이기
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);
        
    }
}
