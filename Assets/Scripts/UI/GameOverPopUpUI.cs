using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUpUI : PopUpUI
{
    [SerializeField] Button RestartButton;
    [SerializeField] Button ExitButton;

    protected override void Awake()
    {
        base.Awake();
        buttons[RestartButton.name].onClick.AddListener(() => { OnRetry(); });
        buttons[ExitButton.name].onClick.AddListener(() => { LeaveGameButton(); });
    }

    void OnRetry()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.Scene.LoadScene("TitleScene");
    }
    public void LeaveGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
