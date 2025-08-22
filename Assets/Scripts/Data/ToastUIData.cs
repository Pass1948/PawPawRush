
using UnityEngine;
public enum AchievementId
{
    TryJump10,        // 나는야 점프왕! : 점프 10회 시도
    LRMove25,       // 왼아가리(예시명) : 좌/우 움직임 횟수 25번
    Coin50,           // 나는 부자가 될꺼야 : 코인 50개 획득
    FistHit,         // 첫 피격
    FistDouble,      // 첫 2배 점수 아이템 획득
    FistHeal,
    FistSpeed,
    FistDeath,


}
[System.Serializable]
public class ToastUIData
{
    public string title;
    public string desc;
    public Sprite icon;
    public AchievementId achievementId; // 업적 ID
}
