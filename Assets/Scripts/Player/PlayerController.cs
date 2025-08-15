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
            // ���� ������ �� ������ ����
            return;
        }

        // TODO: �ʿ��� laneOffset ����
        // �ӽ÷� ����
        float laneOffset = 2.0f;

        currentLane = targetLane;
        targetPosition = new Vector3((currentLane - 1) * laneOffset, 0, 0);
    }

    public void StartRunning()
    {
        isRunning = true;

        // TODO: �ִϸ��̼�
    }

    public void StopRunning()
    {
        isRunning = false;

        // TODO: �ִϸ��̼�
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

        // �ִϸ��̼� �� ���� ���
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
            // ���� ���� �� ����� �ð� ���
            float elapsedTime = Time.time - jumpStartTime;

            // ��� �ð��� �������� ���� �����(ratio)�� 0�� 1 ������ ������ ���
            float ratio = elapsedTime / jumpDuration;

            if (ratio >= 1.0f)
            {
                // ���� �� -> ���� ����
                isJumping = false;

                // TODO: �ִϸ��̼� ���� ���� ����
                // character.animator.SetBool("isJumping", false);

                return 0f; // ���� �� -> ����(y=0)���� ���ư�
            }
            else
            {
                // Sin �Լ��� �̿��� �ε巯�� ������ ������ y�� ���
                // ratio�� 0���� 1�� ���Կ� ���� y���� 0 -> jumpHeight -> 0 ���� ����
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

        // TODO: �ִϸ��̼� �� ���� ���


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
            // �����̵� ���� �� ����� �ð� ���
            float elapsedTime = Time.time - slideStartTime;

            // ��� �ð��� �������� �����̵� �����(ratio)�� 0�� 1 ������ ������ ���
            float ratio = elapsedTime / slideDuration;

            if (ratio >= 1.0f)
            {
                StopSliding();
            }
        }
    }
}
