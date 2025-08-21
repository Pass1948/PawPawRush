using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float boostAmount = 1.5f;           // 속도 증가량
    public float boostDuration = 5f;         // 효과 지속 시간
    [SerializeField] private Color boostColor = Color.yellow; // 깜빡이는 색상

    private PlayerColliderHandler colliderHandler;      // 플레이어 캐릭터 참조
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

                // 맵매니저에서 속도 증가 적용
                MapManager.Instance.ApplySpeedBoost(boostAmount, boostDuration);

                Destroy(gameObject); // 아이템 오브젝트 제거
            }
        }
    }
}
