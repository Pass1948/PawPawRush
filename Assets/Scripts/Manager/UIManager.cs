using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    private Canvas windowCanvas;
    private Canvas popUpCanvas;
    private Canvas toastCanvas;

    private Stack<PopUpUI> popUpStack;

    [Header("Toast알림 조정")]
    [Tooltip("알림 유지 시간(초)")]
    [SerializeField] private float showDuration = 5f;

    private bool useUnscaledTime = true; // 일시정지 무시 여부
    private string toastPrefabPath = "UI/ToastUIBase"; // 프로젝트에 맞게

    private readonly Queue<ToastUIData> toastQueue = new();
    private bool isToastShowing;
    private Coroutine toastRoutine;
    private ToastUIBase toastPrefabCache;

    private void Awake()
    {
        InstantsWindowUI();
        InstantsPopUpUI();
        InstantsToastUI();
        EnsureEventSystem();
    }

    // Scene 변경시 UI를 재생성 메소드
    public void Recreated()
    {
        Clear();
        InstantsWindowUI();
        InstantsPopUpUI();
        InstantsToastUI();
    }
    public void Clear()
    {        // Toast 정리
        if (toastRoutine != null) StopCoroutine(toastRoutine);
        toastQueue.Clear();
        isToastShowing = false;

        // PopUp 정리
        popUpStack.Clear();

        // Canvas 정리
        GameManager.Resource.Destroy(windowCanvas);
        GameManager.Resource.Destroy(popUpCanvas);
        GameManager.Resource.Destroy(toastCanvas);
    }
    public void InstantsWindowUI()
    {
        if (windowCanvas == null)
        {
            windowCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
            windowCanvas.gameObject.name = "WindowCanvas";
            windowCanvas.sortingOrder = 10;
        }
    }
    public void InstantsPopUpUI()
    {
        if (popUpCanvas == null)
        {
            popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
            popUpCanvas.gameObject.name = "PopUpCanvas";
            popUpCanvas.sortingOrder = 100;
            popUpStack = new Stack<PopUpUI>();
        }
    }

    public void InstantsToastUI()
    {
        if (toastCanvas == null)
        {
            toastCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
            toastCanvas.gameObject.name = "ToastCanvas";
            toastCanvas.sortingOrder = 200;
        }
    }

    public void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;

        EventSystem eventSystem = GameManager.Resource.Load<EventSystem>("UI/EventSystem");
        GameManager.Resource.Instantiate(eventSystem,transform);
        DontDestroyOnLoad(eventSystem.gameObject);
    }

    // --------------[WindowUI]--------------

    public T ShowWindowUI<T>(T windowUI) where T : WindowUI
    {
        T ui = GameManager.Pool.GetUI(windowUI);
        ui.transform.SetParent(windowCanvas.transform, false);
        return ui;
    }

    public T ShowWindowUI<T>(string path) where T : WindowUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowWindowUI(ui);
    }

    public void CloseWindowUI(WindowUI windowUI)
    {
        GameManager.Pool.ReleaseUI(windowUI.gameObject);
    }

    public void SelectWindowUI<T>(T windowUI) where T : WindowUI
    {
        windowUI.transform.SetAsLastSibling();
    }

    // --------------[PopUpUI]--------------
    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI
    {
        if (popUpStack.Count > 0)
            popUpStack.Peek().gameObject.SetActive(false);

        T ui = GameManager.Pool.GetUI(popUpUI);
        ui.transform.SetParent(popUpCanvas.transform, false);
        popUpStack.Push(ui);
        return ui;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowPopUpUI(ui);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        GameManager.Pool.Release(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            popUpStack.Peek().gameObject.SetActive(true);
        }
    }
    public void PopUpUIClear()
    {
        // PopUpUI 스택을 비우고 모든 PopUpUI를 반환
        while (popUpStack.Count > 0)
            GameManager.Pool.ReleaseUI(popUpStack.Pop().gameObject);
    }

    // --------------[ToastUI]--------------
    // ===== [공개 API: 데이터로 토스트 요청] =====
    public void EnqueueToast(ToastUIData data)
    {
        if (data == null) { Debug.LogWarning("[UIManager] EnqueueToast: data is null"); return; }
        if (toastCanvas == null) InstantsToastUI();

        toastQueue.Enqueue(data);
        if (!isToastShowing)
            toastRoutine = StartCoroutine(ToastRoutine());
    }

    // ===== [내부: 프리팹 로드 캐시] =====
    private ToastUIBase LoadToastPrefab()
    {
        if (toastPrefabCache != null) return toastPrefabCache;

        if (string.IsNullOrEmpty(toastPrefabPath))
        {
            Debug.LogError("[UIManager] toastPrefabPath is empty");
            return null;
        }

        toastPrefabCache = GameManager.Resource.Load<ToastUIBase>(toastPrefabPath);
        if (toastPrefabCache == null)
            Debug.LogError($"[UIManager] Cannot load ToastUIBase at \"{toastPrefabPath}\"");

        return toastPrefabCache;
    }

    // ===== [내부: 큐 러너 - CanvasGroup/페이드 없음, 단순 대기만] =====
    private IEnumerator ToastRoutine()
    {
        isToastShowing = true;

        while (toastQueue.Count > 0)
        {
            var data = toastQueue.Dequeue();

            var prefab = LoadToastPrefab();
            if (prefab == null) break;

            // 1) 인스턴스 생성, 부모 설정, 데이터 바인딩
            var ui = GameManager.Pool.GetUI(prefab); // ToastUIBase
            var go = ui.gameObject;
            ui.transform.SetParent(toastCanvas.transform, false);
            ui.SetData(data);

            // 2) 즉시 표시 (페이드 없음)
            go.SetActive(true);

            // 3) showDuration 동안 대기 (토스트 간 간격 역할도 겸함)
            if (useUnscaledTime) yield return new WaitForSecondsRealtime(showDuration);
            else yield return new WaitForSeconds(showDuration);

            // 4) 즉시 종료(비활성) 및 정리/반환
            go.SetActive(false);
            ui.Clear();                    // 텍스트/아이콘 비우기(잔상 방지)
            GameManager.Pool.ReleaseUI(go);
        }

        isToastShowing = false;
        toastRoutine = null;
    }
}

