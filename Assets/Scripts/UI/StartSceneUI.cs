using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : WindowUI
{
    [SerializeField] Button SetBotton;
    protected override void Awake()
    {
        base.Awake();
        buttons[SetBotton.name].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<SettingUI>("UI/SetUI"); });
    }
    private void Start()
    {
        GameManager.Sound.PlayBGM("StartBGM");
    }

    private void OnEnable()
    {
       
    }
}
