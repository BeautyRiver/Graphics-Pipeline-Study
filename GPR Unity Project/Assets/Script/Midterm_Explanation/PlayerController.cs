using UnityEngine;
using UnityEngine.UI;

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
    private Vector3 dir;
    public GameObject clearScreen;
    public Text gamemsg;
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
        // ��ũ�� ��ǥ�迡���� �̵� �Է� �ޱ�
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // ī�޶��� ���⿡ ���� �̵� ���� ����
        Vector3 moveHorizontal = Camera.main.transform.right * moveX;
        Vector3 moveVertical = Camera.main.transform.up * moveY;

        // y�� �̵� ���� (�ɼ�)
        moveHorizontal.y = 0;
        moveVertical.y = 0;

        // ���� �̵� ���� ���
        Vector3 movement = (moveHorizontal + moveVertical).normalized * moveSpeed * Time.deltaTime;

        // �÷��̾� ��ġ ������Ʈ
        transform.position += movement;

        // �÷��̾ �̵��ϴ� ������ �ٶ󺸰� �����
        if (movement != Vector3.zero) // �̵� ���Ͱ� 0�� �ƴϸ�
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "End")
        {
            gamemsg.text = "Ŭ����!";
            Time.timeScale = 0;
            clearScreen.SetActive(true);
        }
        if (other.gameObject.tag == "Enemy")
        {
            gamemsg.text = "���ӿ���!";
            Time.timeScale = 0;
            clearScreen.SetActive(true);
        }
    }
}