using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class customWindowUI : WindowUI
{
    [SerializeField] Button startButton;
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        buttons[startButton.name].onClick.AddListener(() => { GameManager.Scene.LoadScene("InGameScene"); });
    }


}
