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
    private Stack<PopUpUI> popUpStack;


    private readonly List<ToastUI> active = new();

    private void Awake()
    {
        InstantsWindowUI();
        InstantsPopUpUI();
        EnsureEventSystem();

        popUpStack = new Stack<PopUpUI>();
    }

    public void Recreated()
    {
        InstantsWindowUI();
        InstantsPopUpUI();
    }

    public void InstantsWindowUI()
    {
        if (windowCanvas == null)
        {
            windowCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            windowCanvas.gameObject.name = "WindowCanvas";
            windowCanvas.sortingOrder = 10;
            DontDestroyOnLoad(windowCanvas.gameObject);
        }
    }
    public void InstantsPopUpUI()
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
        {
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }

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
            PopUpUI curUI = popUpStack.Peek();
            curUI.gameObject.SetActive(true);
        }
    }
    public void PopUpUIClear()
    {
        popUpStack.Clear();
    }


}

