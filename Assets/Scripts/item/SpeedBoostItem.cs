using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float boostAmount = 3f;           // 속도 증가량
    public float boostDuration = 5f;         // 효과 지속 시간
    [SerializeField] private Color boostColor = Color.yellow; // 깜빡이는 색상

    private bool isActive = false;             // 효과가 현재 활성화되어 있는지
    private float timer = 0f;                       // 타이머: 효과가 얼마나 남았는지
    private PlayerColliderHandler colliderHandler;      // 플레이어 캐릭터 참조
    private float originalSpeed;          // 원래 속도 (효과 종료 시 복구용)
    bool isFirst = true; // 첫 활성화 여부

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFirst)
            {
                isFirst= false;
                GameManager.Event.PostNotification(EventType.AchievementUnlocked, this, AchievementId.FistHeal);
            }
            colliderHandler = other.GetComponent<PlayerColliderHandler>();
            if (colliderHandler != null)
            {
                // boostDuration 만큼 무적
                colliderHandler.SetInvincible(boostDuration, boostColor);

                // TODO: 맵 매니저에서 맵이 boostAmount만큼 빨라지게 설정?

                Destroy(gameObject);       // 아이템 오브젝트 제거
            }
        }
    }
}
