using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();   //PlayerHealth 이거 떄문에 주석처리
            //if (playerHealth != null)
            //{
            //    playerHealth.Heal(healAmount); // HP 회복
            //    Destroy(gameObject); // 아이템은 사용 후 제거
            //}
        }
    }
}
