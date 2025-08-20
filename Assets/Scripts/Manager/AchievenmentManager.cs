using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AchievenmentManager : MonoBehaviour, IEventListener
{
    [SerializeField] private AchievementsData achievements; // List<ToastUIData> (각 항목에 achievementId 포함)
    [SerializeField] private bool preventDuplicateToasts = true;

    private Dictionary<AchievementId, ToastUIData> idToData;
    private HashSet<AchievementId> shown;   // 중복 알림 방지용

    private void Awake()
    {
        idToData = new Dictionary<AchievementId, ToastUIData>();
        if (achievements != null && achievements.toastUIInfo != null)
        {
            foreach (var d in achievements.toastUIInfo)
            {
                if (d == null) continue;
                if (!idToData.ContainsKey(d.achievementId))
                    idToData.Add(d.achievementId, d);
            }
        }
        shown = new HashSet<AchievementId>();
    }

    private void OnEnable()
    {
        GameManager.Event.AddListener(EventType.AchievementUnlocked, this);
    }
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Event.RemoveListener(EventType.AchievementUnlocked, this);
    }

    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if (eventType != EventType.AchievementUnlocked) return;

        if (Param is not AchievementId id)
        {
            Debug.LogWarning("[AchievenmentManager] Param이 AchievementId가 아닙니다.", this);
            return;
        }

        if (preventDuplicateToasts && shown.Contains(id)) return;

        if (!idToData.TryGetValue(id, out var data) || data == null)
        {
            Debug.LogWarning($"[AchievenmentManager] 업적 데이터 없음: {id}", this);
            return;
        }

       /* GameManager.UI.EnqueueToast(data); // UI 등장/유지/사라짐/초기화까지 기존 로직이 처리*/
        if (preventDuplicateToasts) shown.Add(id);
    }

    // 디버그/테스트용 직접 호출
    public void ShowToast(AchievementId id)
    {
        OnEvent(EventType.AchievementUnlocked, this, id);
    }
}
