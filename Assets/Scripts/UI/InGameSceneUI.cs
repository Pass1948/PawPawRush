using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSceneUI : WindowUI
{
    [SerializeField] Button SetBotton;
    protected override void Awake()
    {
        base.Awake();
        buttons["SetBotton"].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<SettingUI>("UI/SetUI"); });
    }
}
