using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int MaxLife { get; private set; } = 3;
    public int CurrentLife { get; set; }

    bool isFirst = true; // 첫 활성화 여부
    // 일단 임시로 여기서 목숨 초기화
    private void Start()
    {
        Init();
    }

    // 맵에서 게임 시작 전 호출
    public void Init()
    {
        CurrentLife = MaxLife;
    }

    // 플레이어 목숨 감소
    public void DecreaseLife()
    {
        if (CurrentLife > 0)
        {
            CurrentLife--;
            GameManager.Event.PostNotification(EventType.OnHit,this);
            Debug.Log($"Player Life Decreased: {CurrentLife}");
            if (CurrentLife <= 0)
            {
                // 플레이어가 죽었을 때 처리
                Debug.Log("Player is dead.");
                if(isFirst)
                {
                    isFirst = false;
                    GameManager.Event.PostNotification(EventType.AchievementUnlocked, this, AchievementId.FistDeath);
                    GameManager.UI.ShowPopUpUI<GameOverPopUpUI>("UI/GameOverUI");
                }

            }
        }
    }

    // 플레이어 목숨 증가
    public void IncreaseLife()
    {
        if (CurrentLife < MaxLife)
        {
            CurrentLife++;
            GameManager.Event.PostNotification(EventType.OnHeal, this);
        }
        
        Debug.Log($"{CurrentLife}");
    }
}
