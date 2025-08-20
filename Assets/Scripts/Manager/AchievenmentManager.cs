using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AchievenmentManager : MonoBehaviour, IEventListener
{
    [Header("업적 데이터(SO)")]
    [SerializeField] private AchievementsData achievements;  // toastUIInfo 리스트 보유

    [Header("업적-인덱스 매핑")]
    [SerializeField] private List<ToastUIData> achievementData = new();

    [Header("조건 임계값")]
    [SerializeField] private int jumpTryTarget = 10;
    [SerializeField] private int coinTarget = 50;
    [SerializeField] private int lrMoveTarget = 200;

    // 진행 상황
    private int jumpTry;
    private int coins;
    private int lrMoves;

    private HashSet<AchievementId> unlocked = new();
    private Dictionary<AchievementId, int> map; // enum -> index

    private void Awake()
    {
        // 바인딩 맵 구성
        map = new Dictionary<AchievementId, int>();
        foreach (var b in achievementData)
        {
            if (!map.ContainsKey(b.id)) map.Add(b.id, Mathf.Max(0, b.index));
            else Debug.LogWarning($"Duplicate binding for {b.id}");
        }
    }

    private void OnEnable()
    {
        // 조건 이벤트들 수신
        GameManager.Event.AddListener(EventType.TryJump10, this);
        GameManager.Event.AddListener(EventType.FirstSubItem, this);
        GameManager.Event.AddListener(EventType.Coin50, this);
        GameManager.Event.AddListener(EventType.LRMove100, this);
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Event.RemoveListener(EventType.TryJump10, this);
        GameManager.Event.RemoveListener(EventType.FirstSubItem, this);
        GameManager.Event.RemoveListener(EventType.Coin50, this);
        GameManager.Event.RemoveListener(EventType.LRMove100, this);
    }

    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        switch (eventType)
        {
            case EventType.TryJump10:
                if (++jumpTry >= jumpTryTarget)
                    TryUnlock(AchievementId.TryJump10);
                break;

            case EventType.FirstSubItem:
                // 첫 획득만 업적(Param: 아이템 타입 등 필요시 사용)
                TryUnlock(AchievementId.FirstSubItem);
                break;

            case EventType.Coin50:
                // Param이 int(획득 개수)라면 누적
                coins += Param is int add ? add : 1;
                if (coins >= coinTarget)
                    TryUnlock(AchievementId.Coin50);
                break;

            case EventType.LRMove100:
                // 좌/우 입력 1회당 1증가로 가정
                lrMoves += Param is int step ? step : 1;
                if (lrMoves >= lrMoveTarget)
                    TryUnlock(AchievementId.LRMove100);
                break;
        }
    }

    private void TryUnlock(AchievementId id)
    {
        if (unlocked.Contains(id)) return;
        unlocked.Add(id);

        // 토스트 데이터 찾기 (인덱스)
        if (!map.TryGetValue(id, out int idx))
        {
            Debug.LogWarning($"No binding index for {id}");
            return;
        }
        var list = achievements != null ? achievements.toastUIInfo : null;
        if (list == null || idx < 0 || idx >= list.Count)
        {
            Debug.LogWarning($"Invalid index {idx} for {id}");
            return;
        }

        var data = list[idx];
        // 토스트 등장
        GameManager.UI.EnqueueToast(data);
    }






}
