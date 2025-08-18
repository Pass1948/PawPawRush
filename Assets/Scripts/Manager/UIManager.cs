using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/// UI 관리 및 팝업/윈도우 UI의 생성, 표시, 닫기 등을 담당하는 매니저 클래스
public class UIManager : MonoBehaviour
{
    private Canvas windowCanvas; // 윈도우 UI를 표시할 캔버스
    private Canvas popUpCanvas; // 팝업 UI를 표시할 캔버스
    private Stack<PopUpUI> popUpStack; // 팝업 UI의 스택 (여러 팝업을 관리)

    private void Awake() // 매니저 초기화, 캔버스 및 이벤트 시스템 생성
    {
        InstantsWindowUI();
        InstantsPopUpUI();
        EnsureEventSystem();

        popUpStack = new Stack<PopUpUI>();
    }

    public void Recreated() // UI를 재생성할 때 호출
    {
        InstantsWindowUI();
        InstantsPopUpUI();
    }

    public void InstantsWindowUI() // 윈도우 UI용 캔버스 생성 및 설정
    {
        if (windowCanvas == null)
        {
            windowCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            windowCanvas.gameObject.name = "WindowCanvas";
            windowCanvas.sortingOrder = 10;
            DontDestroyOnLoad(windowCanvas.gameObject);
        }
    }
    public void InstantsPopUpUI() // 팝업 UI용 캔버스 생성 및 설정
    {
        if (popUpCanvas == null)
        {
            popUpCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            popUpCanvas.gameObject.name = "PopUpCanvas";
            popUpCanvas.sortingOrder = 100;
            DontDestroyOnLoad(popUpCanvas.gameObject);

            popUpStack = new Stack<PopUpUI>();
        }
    }

    public void EnsureEventSystem() // 이벤트 시스템이 없을 경우 생성
    {
        if (EventSystem.current != null)
            return;

        EventSystem eventSystem = GameManager.Resource.Load<EventSystem>("UI/EventSystem");
        GameManager.Resource.Instantiate(eventSystem);
        DontDestroyOnLoad(eventSystem.gameObject);
    }


    // --------------[WindowUI]--------------

    public T ShowWindowUI<T>(T windowUI) where T : WindowUI // 윈도우 UI를 표시 (오브젝트 직접 전달)
    {
        T ui = GameManager.Pool.GetUI(windowUI);
        ui.transform.SetParent(windowCanvas.transform, false);
        return ui;
    }

    public T ShowWindowUI<T>(string path) where T : WindowUI // 윈도우 UI를 표시 (리소스 경로로 로드)
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowWindowUI(ui);
    }

    public void CloseWindowUI(WindowUI windowUI) // 윈도우 UI 닫기
    {
        GameManager.Pool.ReleaseUI(windowUI.gameObject);
    }

    public void SelectWindowUI<T>(T windowUI) where T : WindowUI // 윈도우 UI를 최상단으로 선택
    {
        windowUI.transform.SetAsLastSibling();
    }

    // --------------[PopUpUI]--------------
    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI // 팝업 UI를 표시 (리소스 경로로 로드)
    {
        if (popUpStack.Count > 0)
        {
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }

        T ui = GameManager.Pool.GetUI(popUpUI);
        ui.transform.SetParent(popUpCanvas.transform, false);

        popUpStack.Push(ui);

        return ui;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI // 팝업 UI를 표시 (리소스 경로로 로드)
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowPopUpUI(ui);
    }

    public void ClosePopUpUI() // 현재 팝업 UI 닫기
    {
        PopUpUI ui = popUpStack.Pop();
        GameManager.Pool.Release(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI curUI = popUpStack.Peek();
            curUI.gameObject.SetActive(true);
        }
    }

    public void PopUpUIClear() // 모든 팝업 UI 스택 초기화
    {
        popUpStack.Clear();
    }


}

