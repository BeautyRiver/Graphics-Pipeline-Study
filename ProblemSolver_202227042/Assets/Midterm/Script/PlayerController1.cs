using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationDuration = 1.0f; // ȸ���� �ɸ��� �ð�
    public float moveSpeed = 3.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody rb;
    private float rotationTime = 0; // ���� ȸ�� ���� �ð�
    private Quaternion targetRotation; // ��ǥ ȸ��
    private bool isRotating = false; // ȸ�� ������ ����

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
            // ��ǥ ȸ�� ���
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            // ���� ȸ������ ��ǥ ȸ������ �ε巴�� ����
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
