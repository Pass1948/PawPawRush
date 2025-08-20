using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
public enum AchievementId
{
    TryJump10,        // 나는야 점프왕! : 점프 10회 시도
    FirstSubItem,     // 시간은 금이라고 친구 : 자석아이템 첫 획득
    Coin50,           // 나는 부자가 될꺼야 : 코인 50개 획득
    LRMove100        // 왼아가리(예시명) : 좌/우 움직임 횟수 200번
}
[System.Serializable]
public class ToastUIData
{
    public string title;
    public string desc;
    public Sprite icon;
    public AchievementId achievementId; // 업적 ID
}
