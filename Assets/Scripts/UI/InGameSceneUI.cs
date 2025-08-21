using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSceneUI : WindowUI
{
    [SerializeField] Button SetBotton;
    [SerializeField] TMP_Text countText;


    //Hp 이미지
    

    // 점수


    protected override void Awake()
    {
        base.Awake();
        buttons[SetBotton.name].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<SettingUI>("UI/SetUI"); });
    }



}
