using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationDuration = 1.0f; // ȸ���� �ɸ��� �ð�
    public float moveSpeed = 3.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody rb;
    private float rotationTime = 0; // ���� ȸ�� ���� �ð�
    private Quaternion targetRotation; // ��ǥ ȸ��
    private bool isRotating = false; // ȸ�� ������ ����
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        RotateCamera();
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Get input from the vertical and horizontal axis
        float moveZ = Input.GetAxisRaw("Vertical"); // Use 'Vertical' for z-axis movement
        float moveX = Input.GetAxisRaw("Horizontal"); // Use 'Horizontal' for x-axis movement

        // Calculate the movement direction based on the camera's orientation
        Vector3 moveDirection = (Camera.main.transform.forward * moveZ + Camera.main.transform.right * moveX).normalized;
        moveDirection.y = 0f; // Ignore any vertical movement in the camera's direction

        // Apply the movement to the player's position
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    void RotateCamera()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isRotating)
        {
            targetRotation = cameraTransform.rotation * Quaternion.Euler(0, -90, 0);
            rotationTime = 0;
            isRotating = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) && !isRotating)
        {
            targetRotation = cameraTransform.rotation * Quaternion.Euler(0, 90, 0);
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