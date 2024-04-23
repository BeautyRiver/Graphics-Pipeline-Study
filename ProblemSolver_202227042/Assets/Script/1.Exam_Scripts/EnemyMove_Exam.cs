using UnityEngine;

public class EnemyMove_Exam : MonoBehaviour
{
    public GameObject target;
    public float speed;
    private float currentSpeed;
    public LayerMask wallLayer;
    private void Update()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), directionToTarget, Vector3.Distance(target.transform.position, transform.position), wallLayer))
        {
            currentSpeed = 0;
        }
        else
        {
            currentSpeed = speed;
        }
        // 타겟을 향해 움직이기
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);


        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), directionToTarget, Color.red);
    }
}
