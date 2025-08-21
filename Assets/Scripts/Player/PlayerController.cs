using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    // Animator parameter
    private static int startHash = Animator.StringToHash("Greeting");
    private static int runStartHash = Animator.StringToHash("RunStart");
    private static int movingHash = Animator.StringToHash("Moving");
    private static int jumpingHash = Animator.StringToHash("Jumping");
    private static int slidingHash = Animator.StringToHash("Sliding");
    public static int DeadHash = Animator.StringToHash("Dead");

    // Components
    public PlayerColliderHandler ColliderHandler { get; private set; }
    private PlayerColliderHandler colliderHandler;
    public Animator Animator { get; private set; }
    private Animator animator;
    private AudioSource loopingAudioSource;
    private AudioSource sfxAudioSource;

    [Header("Movement")]
    [SerializeField] private float laneChangeSpeed = 1.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float jumpCooldown = 0.8f;
    [SerializeField] private float slideDuration = 0.5f;

    [Header("Audio")]
    [SerializeField, Range(0f, 1f)]
    private float runningSoundVolume = 0.03f;

    // Player State
    private bool isRunning;
    private bool isJumping;
    private bool isSliding;

    // Jumping & Sliding time
    private float jumpStartTime;
    private float lastJumpTime = -1f;
    private float slideStartTime;

    private Vector3 targetPosition;
    private int currentLane = STARTING_LANE;

    // Constants
    private const int STARTING_LANE = 1;
    private const float LANE_OFFSET = 2.0f; // 레인 간격

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colliderHandler = GetComponent<PlayerColliderHandler>();

        targetPosition = transform.position;
    }

    private void OnEnable()
    {
        MapManager.OnMapMovementStarted += ManageRunningSound; // 이벤트 구독
    }

    private void OnDisable()
    {
        MapManager.OnMapMovementStarted -= ManageRunningSound; // 이벤트 구독 해제
    }

    private void Start()
    {
        loopingAudioSource = GameManager.Player.PlayerCharacter.LoopingAudioSource;
        sfxAudioSource = GameManager.Player.PlayerCharacter.SfxAudioSource;

        // temp
        //StartCoroutine(WaitToStart());
    }

    private void Update()
    {
        if(!isRunning)
        {
            return;
        }

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

        // 달리기 소리가 재생 중일 때 맵 속도에 맞춰 pitch 조절
        if (loopingAudioSource.isPlaying && loopingAudioSource.clip == GameManager.Player.PlayerCharacter.RunningSound)
        {
            // MapManager에서 현재 속도와 원래 속도를 가져옴
            float currentSpeed = MapManager.Instance.MapMovement.movementSpeed;
            float originalSpeed = MapManager.Instance.OrigMapMovementSpeed;

            // originalSpeed가 0보다 클 때만 계산 (0으로 나누는 오류 방지)
            if (originalSpeed > 0)
            {
                // 원래 속도 대비 현재 속도의 비율을 계산하여 pitch 값으로 사용
                float targetPitch = currentSpeed / originalSpeed;

                // pitch가 너무 낮아지는 것을 방지하기 위해 최소값 설정
                loopingAudioSource.pitch = Mathf.Max(0.8f, targetPitch);
            }
            else
            {
                // 속도 값이 유효하지 않을 경우 기본 pitch로 설정
                loopingAudioSource.pitch = 0f;
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

        currentLane = targetLane;
        targetPosition = new Vector3((currentLane - 1) * LANE_OFFSET, targetPosition.y, targetPosition.z);
    }

    public IEnumerator PrepareForGameStart(float countdownDuration)
    {
        StopRunning();

        // 카메라 방향 쪽으로 플레이어 회전
        Tween lookAtCamera = transform.DORotate(new Vector3(0, 180, 0), 0.5f);
        yield return lookAtCamera.WaitForCompletion(); // 회전이 끝날 때까지 코루틴을 잠시 대기

        animator.Play(startHash);

        // 카운트다운 시간만큼 대기
        yield return new WaitForSeconds(countdownDuration);

        // 다시 정면으로 플레이어 회전
        // 원래 방향(Y축 0도)으로 돌아옴 (0.3초 동안)
        Tween lookForward = transform.DORotate(Vector3.zero, 0.3f);
        yield return lookForward.WaitForCompletion(); // 회전이 끝날 때까지 다시 대기

        StartRunning();
    }

    public void StartRunning()
    {
        isRunning = true;

        animator.Play(runStartHash);
        animator.SetBool(movingHash, true);
    }

    public void StopRunning()
    {
        isRunning = false;

        animator.SetBool(movingHash, false);

        ManageRunningSound();
    }

    public void HandleJump()
    {
        if (!isRunning || isJumping || Time.time < lastJumpTime + +jumpCooldown)
        {
            return;
        }


        if (isSliding)
        {
            StopSliding();
        }

        lastJumpTime = Time.time;
        isJumping = true;
        jumpStartTime = Time.time;

        // 애니메이션 및 사운드 재생
        sfxAudioSource.PlayOneShot(GameManager.Player.PlayerCharacter.JumpSound); // 슬라이드 사운드 재생
        animator.SetBool(jumpingHash, true);

        ManageRunningSound();
    }

    public void StopJumping()
    {
        isJumping = false;

        animator.SetBool(jumpingHash, false);

        ManageRunningSound();
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
                StopJumping();

                return targetPosition.y; // 점프 끝 -> 지면으로 돌아감
            }
            else
            {
                // Sin 함수를 이용해 부드러운 포물선 형태의 y값 계산
                // ratio가 0에서 1로 변함에 따라 y값은 0 -> jumpHeight -> 0 으로 변함
                return targetPosition.y + Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }

        return targetPosition.y;
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
        animator.SetBool(slidingHash, true);

        colliderHandler.Slide(isSliding);

        ManageRunningSound();
    }

    public void StopSliding()
    {
        if(isSliding)
        {
            animator.SetBool(slidingHash, false);
            isSliding = false;

            colliderHandler.Slide(isSliding);

            ManageRunningSound();
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

    private void ManageRunningSound()
    {
        bool shouldPlayRunningSound = isRunning && !isJumping && !isSliding;

        if (shouldPlayRunningSound)
        {
            if (!loopingAudioSource.isPlaying)
            {
                loopingAudioSource.clip = GameManager.Player.PlayerCharacter.RunningSound;
                loopingAudioSource.loop = true;
                loopingAudioSource.volume = runningSoundVolume;
                loopingAudioSource.Play();
            }
        }
        else
        {
            if (loopingAudioSource.clip == GameManager.Player.PlayerCharacter.RunningSound)
            {
                loopingAudioSource.Stop();
                //loopingAudioSource.pitch = 1f;    // pitch를 기본값 1로 초기화
                //loopingAudioSource.volume = 1f;   // 볼륨을 기본값 1로 초기화
            }
        }
    }
}
