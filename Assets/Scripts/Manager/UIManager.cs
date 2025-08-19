using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class UIManager : MonoBehaviour
{

    private Canvas windowCanvas;
    private Canvas popUpCanvas;
    private Canvas toastCanvas;

    private Stack<PopUpUI> popUpStack;

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
        InstantsWindowUI();
        InstantsPopUpUI();
        InstantsToastUI();
    }
    public void Clear()
    {
        popUpStack.Clear();
        GameManager.Resource.Destroy(windowCanvas);
        GameManager.Resource.Destroy(popUpCanvas);
        GameManager.Resource.Destroy(toastCanvas);
    }
    public void InstantsWindowUI()
    {
        if (windowCanvas == null)
        {
            windowCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            windowCanvas.gameObject.name = "WindowCanvas";
            windowCanvas.sortingOrder = 10;
        }
    }
    public void InstantsPopUpUI()
    {
        if (popUpCanvas == null)
        {
            popUpCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            popUpCanvas.gameObject.name = "PopUpCanvas";
            popUpCanvas.sortingOrder = 100;
            popUpStack = new Stack<PopUpUI>();
        }
    }

    public void InstantsToastUI()
    {
        if (toastCanvas == null)
        {
            toastCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            toastCanvas.gameObject.name = "ToastCanvas";
            toastCanvas.sortingOrder = 200;
        }
    }

    public void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;

        EventSystem eventSystem = GameManager.Resource.Load<EventSystem>("UI/EventSystem");
        GameManager.Resource.Instantiate(eventSystem);
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
}

