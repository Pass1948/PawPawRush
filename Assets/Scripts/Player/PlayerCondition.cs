using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int MaxLife { get; private set; } = 3;
    public int CurrentLife { get; set; }

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
        }
    }

    // 플레이어 목숨 증가
    public void IncreaseLife()
    {
        if (CurrentLife < MaxLife)
        {
            CurrentLife++;
        }
    }
}
