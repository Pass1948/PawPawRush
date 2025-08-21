using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    bool isFirst = true; // 첫 활성화 여부
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            if (isFirst)
            {
                isFirst = false;
                GameManager.Event.PostNotification(EventType.AchievementUnlocked, this, AchievementId.FistHeal);
            }
            PlayerCondition playerHealth = other.GetComponent<PlayerCondition>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseLife(); // HP 회복

                // TODO: 오브젝트 풀링
                Destroy(gameObject); // 아이템은 사용 후 제거
            }
        }
    }
}
