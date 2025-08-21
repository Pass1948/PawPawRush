using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleWindowUI : WindowUI
{
    [SerializeField] 
    Button startButton;
    protected override void Awake()
    {
        base.Awake();
        buttons[startButton.name].onClick.AddListener(() => { StartGame(); });
    }

    private void StartGame()
    {
        GameManager.Scene.LoadScene("InGameScene");
    }
}
