using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationDuration = 1.0f; // 회전에 걸리는 시간
    public float moveSpeed = 3.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody rb;
    private float rotationTime = 0; // 현재 회전 진행 시간
    private Quaternion targetRotation; // 목표 회전
    private bool isRotating = false; // 회전 중인지 여부
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
        // 스크린 좌표계에서의 이동 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 카메라의 방향에 따라 이동 벡터 조정
        Vector3 moveHorizontal = Camera.main.transform.right * moveX;
        Vector3 moveVertical = Camera.main.transform.up * moveY;

        // y축 이동 제거 (옵션)
        moveHorizontal.y = 0;
        moveVertical.y = 0;

        // 최종 이동 벡터 계산
        Vector3 movement = (moveHorizontal + moveVertical).normalized * moveSpeed * Time.deltaTime;

        // 플레이어 위치 업데이트
        transform.position += movement;

        // 플레이어가 이동하는 방향을 바라보게 만들기
        if (movement != Vector3.zero) // 이동 벡터가 0이 아니면
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
            gamemsg.text = "클리어!";
            Time.timeScale = 0;
            clearScreen.SetActive(true);
        }
        if (other.gameObject.tag == "Enemy")
        {
            gamemsg.text = "게임오버!";
            Time.timeScale = 0;
            clearScreen.SetActive(true);
        }
    }
}