using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    public float boostAmount = 3f;           // 속도 증가량
    public float boostDuration = 5f;         // 효과 지속 시간

    private bool isActive = false;             // 효과가 현재 활성화되어 있는지
    private float timer = 0f;                       // 타이머: 효과가 얼마나 남았는지
    private CharacterInputController character;      // 플레이어 캐릭터 참조
    private float originalSpeed;          // 원래 속도 (효과 종료 시 복구용)

    private void Update()
    {
        if (!isActive || character == null)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            character.moveSpeed = originalSpeed;    // 효과 종료: 속도 복구, 무적 해제
            character.isInvincible = false;
            isActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            character = other.GetComponent<CharacterInputController>();
            if (character != null)
            {
                ApplySpeedAndInvincibility();
                Destroy(gameObject);       // 아이템 오브젝트 제거
            }
        }
    }

    private void ApplySpeedAndInvincibility()
    {
        originalSpeed = character.moveSpeed;   // 현재 속도 저장
        character.moveSpeed += boostAmount;    // 속도 증가
        character.isInvincible = true;        // 무적 상태 ON
        timer = boostDuration;                // 타이머 시작
        isActive = true;                     // 효과 활성화
    }
}

internal class CharacterInputController
{
    internal bool isInvincible;
    internal float moveSpeed;
}