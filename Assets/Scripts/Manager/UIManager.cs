using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    // 레이어에 따른 기준 캔버스 좋음
    private Canvas windowCanvas;
    private Canvas popUpCanvas;
    private Canvas toastCanvas;

    private Stack<PopUpUI> popUpStack;

    [Header("Toast알림 조정")]
    [Tooltip("알림 유지 시간(초)")]
    [SerializeField] private float showDuration = 5f;

    private bool useUnscaledTime = true; // 일시정지 무시 여부
    // 개별 오브젝트의 위치를 데이터로??
    private string toastPrefabPath = "UI/ToastUIBase"; // 프로젝트에 맞게

    // 기능자체는 좋은나 매니저가 커질 것 같다
    // 분리모듈을 준비하는것도 방법
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
    
    // window, popUp, toast 기능 동일 - 재사용 가능하게구성 
    public void InstantsWindowUI()
    {
        if (windowCanvas == null)
        {
            // 메인 경로는 상수로
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

    // 체크 좋음
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

    // 사용하는데서 패스를 넘기면 활용이 힘듬
    // 특히 경로는 매니저에서 관리, key 값만 전달하자
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
        
        // 스택형 팝업에 대한 이해 좋음
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
    
    // 팝업 클리어가 필요한 경우가 종종 있음 - Good
    // 모두 팝업말고 특정 팝업이 나올때까지 Pop 하는 기능도 있으면 좋음
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

        // 큐에 대한 이해
        toastQueue.Enqueue(data);
        
        // 중복 토스트 실행 체크 좋음
        // 코루틴 관리를 위해 toastRoutine 에 저장하는 것 좋음
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

