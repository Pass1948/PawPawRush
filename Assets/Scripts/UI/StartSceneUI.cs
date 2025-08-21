using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : WindowUI
{
    [SerializeField] Button setBotton;
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button startButton2;

    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject customUI;
    protected override void Awake()
    {
        base.Awake();
        buttons[setBotton.name].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<SettingUI>("UI/SetUI"); });
        buttons[startButton.name].onClick.AddListener(() => { ChangeUI(); });
        buttons[exitButton.name].onClick.AddListener(() => {LeaveGameButton();});
    }
    private void Start()
    {
        GameManager.Sound.PlayBGM("StartBGM");
    }

    void ChangeUI()
    {
        titleUI.SetActive(false);
        customUI.SetActive(true);
    }

    private void LeaveGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
