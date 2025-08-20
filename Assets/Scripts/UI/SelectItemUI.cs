using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : WindowUI
{
    [SerializeField] Button button1;
    [SerializeField] Button button2;
    [SerializeField] Button button3;
    [SerializeField] Button button4;
    [SerializeField] Button button5;
    [SerializeField] Button button6;
    [SerializeField] Button button7;
    [SerializeField] Button button8;
    [SerializeField] Button button9;

    protected override void Awake()
    {
        base.Awake();
        buttons[button1.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(1);  });
        buttons[button2.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(2);  });
        buttons[button3.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(3);  });
        buttons[button4.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(4);  });
        buttons[button5.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(5);  });
        buttons[button6.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(6);  });
        buttons[button7.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(7);  });
        buttons[button8.name].onClick.AddListener(() => {GameManager.Player.ChangeAndSaveEquippedAccessory(8);  });
        buttons[button9.name].onClick.AddListener(() => { GameManager.Player.ChangeAndSaveEquippedAccessory(9);  });
    }
}
