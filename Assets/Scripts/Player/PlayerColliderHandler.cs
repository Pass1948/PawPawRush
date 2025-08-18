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
    [SerializeField] private List<Renderer> playerRenderers; // 플레이어 렌더러 컴포넌트
    private Color[] originalColors; // 원래 색상 저장용

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
    private const string COINS_TAG = "Coin";
    private const string OBSTACLE_TAG = "Obstacle";
    private const string ITEM_TAG = "Item";

    private void Awake()
    {
        
    }

    private void Start()
    {
        // 컴포넌트 캐싱
        playerController = GameManager.Player.PlayerCharacter.PlayerController;
        playerCondition = GameManager.Player.PlayerCharacter.PlayerCondition;

        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // 렌더러 원래 색상 저장
        originalColors = new Color[playerRenderers.Count];
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            if (playerRenderers[i] != null)
            {
                originalColors[i] = playerRenderers[i].material.color;
            }
        }
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
        Debug.Log("Invincible Timer Started");
        isInvincible = true;

        float time = 0;
        float lastBlink = 0.0f;

        // 무적 시간
        while (time < invincibleDuration)
        {
            yield return null;
            time += Time.deltaTime;
            lastBlink += Time.deltaTime;

            if (blinkPeriod < lastBlink)
            {
                lastBlink = 0;

                for(int i = 0; i < playerRenderers.Count; i++)
                {
                    if (playerRenderers[i] == null)
                    {
                        continue; // 렌더러가 없으면 다음으로
                    }

                    // 현재 색상이 원래 색상이면 무적 색상으로 아니면 원래 색상으로 변경
                    if (playerRenderers[i].material.color == originalColors[i])
                    {
                        playerRenderers[i].material.color = invincibleColor;
                    }
                    else
                    {
                        playerRenderers[i].material.color = originalColors[i];
                    }
                }
            }
        }

        for(int i = 0; i < playerRenderers.Count; i++)
        {
            if (playerRenderers[i] != null)
            {
                playerRenderers[i].material.color = originalColors[i]; // 원래 색상으로 복원
            }
        }

        Debug.Log("Invincible Timer Ended");
        isInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(COINS_TAG)) // 코인 충돌 처리(아이템 구현 후 적용)
        {
            Debug.Log("Coin Collected");
        }
        else if(other.CompareTag(OBSTACLE_TAG)) // 장애물 충돌 처리(맵 구현하면서 적용)
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
        else if(other.CompareTag(ITEM_TAG))
        {
            playerController.UseItem();
        }
    }
}
