using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AchievenmentManager : MonoBehaviour, IEventListener
{
    [SerializeField] private AchievementsData achievements; // List<ToastUIData> (각 항목에 achievementId 포함)
    private string achievementsPath = "Data/UI/Achievement"; // Resources/Data/Achievement.asset

    [SerializeField] private bool preventDuplicateToasts = true;

    private Dictionary<AchievementId, ToastUIData> idToData;
    private HashSet<AchievementId> shown;   // 중복 알림 방지용
    private bool initialized;
    private void Awake()
    {
        Initialize();
    }

    // 외부에서 나중에 주입해도 되도록
    public void SetAchievements(AchievementsData data)
    {
        achievements = data;
        initialized = false;
        Initialize();
    }

    private void Initialize()
    {
        if (initialized) return;

        // 1) SO 없으면 Resources에서 로드
        if (achievements == null && !string.IsNullOrEmpty(achievementsPath))
            achievements = GameManager.Resource.Load<AchievementsData>(achievementsPath);

        // 2) 맵 구성
        idToData = new Dictionary<AchievementId, ToastUIData>();
        if (achievements != null && achievements.toastUIInfo != null)
        {
            foreach (var d in achievements.toastUIInfo)
            {
                if (d == null) continue;
                if (!idToData.TryAdd(d.achievementId, d))
                    Debug.LogWarning($"[AchievenmentManager] 중복 achievementId: {d.achievementId}", this);
            }
        }
        else
        {
            Debug.LogError("[AchievenmentManager] AchievementsData가 비어있습니다. 인스펙터로 할당하거나 achievementsPath를 확인하세요.", this);
        }

        shown = new HashSet<AchievementId>();
        initialized = true;

        // (선택) 누락된 ID 빠르게 확인
        LogMissingIds();
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
        if (!initialized) Initialize();

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

        GameManager.UI.EnqueueToast(data);
        if (preventDuplicateToasts) shown.Add(id);
    }

    private void LogMissingIds()
    {
        foreach (AchievementId id in System.Enum.GetValues(typeof(AchievementId)))
        {
            if (idToData == null || !idToData.ContainsKey(id))
                Debug.LogWarning($"[AchievenmentManager] SO에 매핑되지 않은 업적 ID: {id}", this);
        }
    }
}
