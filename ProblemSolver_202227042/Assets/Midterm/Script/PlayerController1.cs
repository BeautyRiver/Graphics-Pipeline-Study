using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationDuration = 1.0f; // 회전에 걸리는 시간
    public float moveSpeed = 3.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody rb;
    private float rotationTime = 0; // 현재 회전 진행 시간
    private Quaternion targetRotation; // 목표 회전
    private bool isRotating = false; // 회전 중인지 여부

    void Update()
    {
        RotateCamera();
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            // 목표 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            // 현재 회전에서 목표 회전까지 부드럽게 보간
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, turnSpeed);
        }
    }

    void RotateCamera()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isRotating)
        {
            targetRotation = cameraTransform.rotation * Quaternion.Euler(0, 0, -90);
            rotationTime = 0;
            isRotating = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) && !isRotating)
        {
            targetRotation = cameraTransform.rotation * Quaternion.Euler(0, 0, 90);
            rotationTime = 0;
            isRotating = true;
        }

        if (isRotating)
        {
            if (rotationTime < rotationDuration)
            {
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, rotationTime / rotationDuration);
                rotationTime += Time.deltaTime;
            }
            else
            {
                cameraTransform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
}
