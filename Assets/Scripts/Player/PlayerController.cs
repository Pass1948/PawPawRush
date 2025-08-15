using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    // Animator parameter


    // Components
    private PlayerColliderHandler colliderHandler;

    [Header("Movement")]
    [SerializeField] private float laneChangeSpeed = 1.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float slideDuration = 0.5f;

    // Player State
    private bool isRunning = true;
    private bool isJumping;
    private bool isSliding;

    // Jumping & Sliding time
    private float jumpStartTime;
    private float slideStartTime;

    private Vector3 targetPosition = Vector3.zero;
    private int currentLane = STARTING_LANE;

    // Constants
    private const int STARTING_LANE = 1;

    private void Awake()
    {

    }

    private void Start()
    {
        colliderHandler = GetComponent<PlayerColliderHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            HandleJump();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (!isSliding)
            {
                HandleSlide();
            }
        }

        Vector3 finalTargetPosition = targetPosition;

        CalculateSlide();
        finalTargetPosition.y = CalculateJumpY();

        transform.position = Vector3.MoveTowards(transform.localPosition, finalTargetPosition, laneChangeSpeed * Time.deltaTime);
    }

    public void ChangeLane(int direction)
    {
        if (!isRunning)
        {
            return;
        }

        int targetLane = currentLane + direction;

        if (targetLane < 0 || targetLane > 2)
        {
            // 레인 밖으로 못 나가게 무시
            return;
        }

        // TODO: 맵에서 laneOffset 조절
        // 임시로 설정
        float laneOffset = 2.0f;

        currentLane = targetLane;
        targetPosition = new Vector3((currentLane - 1) * laneOffset, 0, 0);
    }

    public void StartRunning()
    {
        isRunning = true;

        // TODO: 애니메이션
    }

    public void StopRunning()
    {
        isRunning = false;

        // TODO: 애니메이션
    }

    public void HandleJump()
    {
        if (!isRunning || isJumping)
        {
            return;
        }


        if (isSliding)
        {
            StopSliding();
        }


        isJumping = true;
        jumpStartTime = Time.time;

        // 애니메이션 및 사운드 재생
        //character.animator.SetFloat(s_JumpingSpeedHash, animSpeed);
        //character.animator.SetBool(s_JumpingHash, true);
        //m_Audio.PlayOneShot(character.jumpSound);
    }

    public void StopJumping()
    {

    }

    public float CalculateJumpY()
    {
        if (isJumping)
        {
            // 점프 시작 후 경과된 시간 계산
            float elapsedTime = Time.time - jumpStartTime;

            // 경과 시간을 기준으로 점프 진행률(ratio)을 0과 1 사이의 값으로 계산
            float ratio = elapsedTime / jumpDuration;

            if (ratio >= 1.0f)
            {
                // 점프 끝 -> 상태 변경
                isJumping = false;

                // TODO: 애니메이션 상태 변경 로직
                // character.animator.SetBool("isJumping", false);

                return 0f; // 점프 끝 -> 지면(y=0)으로 돌아감
            }
            else
            {
                // Sin 함수를 이용해 부드러운 포물선 형태의 y값 계산
                // ratio가 0에서 1로 변함에 따라 y값은 0 -> jumpHeight -> 0 으로 변함
                return Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }

        return 0f;
    }

    public void HandleSlide()
    {
        if (!isRunning || isSliding)
        {
            return;
        }

        if (isJumping)
        {
            StopJumping();
        }

        isSliding = true;
        slideStartTime = Time.time;

        // TODO: 애니메이션 및 사운드 재생


        colliderHandler.Slide(isSliding);
    }

    public void StopSliding()
    {
        if(isSliding)
        {
            isSliding = false;
            colliderHandler.Slide(isSliding);
        }
    }

    public void CalculateSlide()
    {
        if (isSliding)
        {
            // 슬라이딩 시작 후 경과된 시간 계산
            float elapsedTime = Time.time - slideStartTime;

            // 경과 시간을 기준으로 슬라이딩 진행률(ratio)을 0과 1 사이의 값으로 계산
            float ratio = elapsedTime / slideDuration;

            if (ratio >= 1.0f)
            {
                StopSliding();
            }
        }
    }
}
