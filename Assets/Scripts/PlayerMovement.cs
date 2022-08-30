using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f; // 앞뒤 움직임의 속도
  public float rotateSpeed = 180f; // 좌우 회전 속도


  private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
  private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
  private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

  private void Start()
  {
    // 사용할 컴포넌트들의 참조를 가져오기
    playerInput = GetComponent<PlayerInput>();
    playerRigidbody = GetComponent<Rigidbody>();
    playerAnimator = GetComponent<Animator>();
  }

  // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
  private void FixedUpdate()
  {
    // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
    Rotate();
    Move();

    playerAnimator.SetFloat("Move", playerInput.move);
  }

  // 입력값에 따라 캐릭터를 앞뒤로 움직임
  private void Move()
  {
    Vector3 moveDistance = playerInput.move * moveSpeed * Time.deltaTime * transform.forward;
    playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
  }

  // 입력값에 따라 캐릭터를 좌우로 회전
  private void Rotate()
  {
    //float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
    //playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0f, turn, 0f);
    //Debug.Log($"player rotation angle : {playerRigidbody.rotation.eulerAngles.x}");

    // 현재 마우스 포지션에서 정면방향 * 10으로 이동한 위치의 월드좌표 구하기
    //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(playerInput.mousePos.x, Camera.main.nearClipPlane, -playerInput.mousePos.y));
    //Debug.Log(playerInput.mousePos.x + " / " + mouseWorldPosition.x);

    // Atan2를 이용하면 높이와 밑변(tan)으로 라디안(Radian)을 구할 수 있음
    // Mathf.Rad2Deg를 곱해서 라디안(Radian)값을 도수법(Degree)으로 변환
    float angle = Mathf.Atan2(
        this.transform.position.y - mouseWorldPosition.y,
        this.transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;

    // angle이 0~180의 각도라서 보정
    float final = -(angle + 90f);
    // 로그를 통해서 값 확인
    //Debug.Log(angle + " / " + final);

    // Y축 회전
    this.transform.rotation = Quaternion.Euler(new Vector3(0f, final, 0f));
  }
}