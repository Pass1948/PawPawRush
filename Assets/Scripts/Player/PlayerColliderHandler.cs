using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour
{
    // Animator parameter
    private static int hitHash = Animator.StringToHash("Hit");

    // Components
    private PlayerController playerController;
    private PlayerCondition playerCondition;
    private BoxCollider boxCollider;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private Renderer playerRenderer; // 플레이어 렌더러 컴포넌트
    [SerializeField] private Color originalColor;     // 플레이어 원래 색상

    [Header("Sound")]
    public AudioClip coinSound;

    [Header("Invincibility Settings")]
    [SerializeField]
    private float invincibleDuration = 2.0f; // 무적 지속 시간
    [SerializeField]
    private Color invincibleColor = Color.red;  // 무적 상태일 때 깜빡임 색상
    [SerializeField]
    private float blinkPeriod = 0.1f;           // 깜빡이는 주기

    private bool isInvincible;

    private readonly Vector3 slidingColliderScale = new Vector3(1.0f, 0.5f, 1.0f);
    private readonly Vector3 notSlidingColliderScale = new Vector3(1.0f, 2.0f, 1.0f);

    // Constants
    private const int COINS_LAYER_INDEX = 8;
    private const int OBSTACLE_LAYER_INDEX = 9;

    private void Awake()
    {
        // 컴포넌트 캐싱
        playerController = GameManager.Player.PlayerCharacter.PlayerController;
        playerCondition = GameManager.Player.PlayerCharacter.PlayerCondition;

        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Slide(bool isSliding)
    {
        if (isSliding)
        {
            boxCollider.size = Vector3.Scale(boxCollider.size, slidingColliderScale);
            boxCollider.center = boxCollider.center - new Vector3(0.0f, boxCollider.size.y * 0.5f, 0.0f);
        }
        else
        {
            boxCollider.center = boxCollider.center + new Vector3(0.0f, boxCollider.size.y * 0.5f, 0.0f);
            boxCollider.size = Vector3.Scale(boxCollider.size, notSlidingColliderScale);
        }
    }

    public void SetInvincible()
    {
        StartCoroutine(InvincibleTimer());
    }

    private IEnumerator InvincibleTimer()
    {
        isInvincible = true;

        float time = 0;
        float lastBlink = 0.0f;

        Color origColor = playerRenderer.material.color;

        // 무적 시간
        while (time < invincibleDuration && isInvincible)
        {
            yield return null;
            time += Time.deltaTime;
            lastBlink += Time.deltaTime;

            if (blinkPeriod < lastBlink)
            {
                lastBlink = 0;

                // 현재 색상이 원래 색상이면 무적 색상으로 아니면 원래 색상으로 변경
                if (playerRenderer.material.color == origColor)
                {
                    playerRenderer.material.color = invincibleColor;
                }
                else
                {
                    playerRenderer.material.color = origColor;
                }
            }
        }

        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }

        isInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == COINS_LAYER_INDEX) // 코인 충돌 처리(아이템 구현 후 적용)
        {

        }
        else if(other.gameObject.layer == OBSTACLE_LAYER_INDEX) // 장애물 충돌 처리(맵 구현하면서 적용)
        {
            if(isInvincible)
            {
                return;
            }

            playerController.StopRunning();

            other.enabled = false; // 장애물 콜라이더 비활성화

            playerCondition.DecreaseLife();

            animator.SetTrigger(hitHash);

            if(playerCondition.CurrentLife > 0)
            {
                // TODO: 피격 사운드 재생
                //audioSource.PlayOneShot(GameManager.Player.PlayerCharacter.HitSound);

                SetInvincible();
            }
            else // 플레이어 죽음
            {
                // TODO: 죽음 사운드 재생
                //audioSource.PlayOneShot(GameManager.Player.PlayerCharacter.DeathSound);

                // 플레이어 죽고 데이터를 맵 매니저나 게임 매니저에 전달?

                // 플레이어가 죽으면 스테이지 종료라서 맵 매니저에서 플레이어 죽음 처리?
            }
        }
    }
}
