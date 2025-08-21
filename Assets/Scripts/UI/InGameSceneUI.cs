using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameSceneUI : WindowUI, IEventListener
{
    [SerializeField] Button SetBotton;
    [SerializeField] TMP_Text timerText;

    //Hp 이미지
    [SerializeField] GameObject heart1;
    [SerializeField] GameObject heart2;
    [SerializeField] GameObject heart3;
    [SerializeField] GameObject nullHeart1;
    [SerializeField] GameObject nullHeart2;
    [SerializeField] GameObject nullHeart3;

    // 점수
    [SerializeField] TMP_Text scoreText;
    float countdownStart = 5;

    protected override void Awake()
    {
        base.Awake();
        buttons[SetBotton.name].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<SettingUI>("UI/SetUI"); });
    }

    private void Update()
    {
        scoreText.text = GameManager.Score.BaseScore.ToString();
        StartCountdow();
    }

    public void StartCountdow()
    {
        countdownStart -= Time.deltaTime;
        timerText.text = (countdownStart).ToString("N0");
        if (countdownStart <= 0)
        {
            timerText.gameObject.SetActive(false);
        }
    }

    void ChangeHP()
    {
        switch (GameManager.Player.PlayerCharacter.PlayerCondition.CurrentLife)
        {
            case 3:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
                nullHeart1.SetActive(false);
                nullHeart2.SetActive(false);
                nullHeart3.SetActive(false);
                break;
            case 2:
                heart1.SetActive(false);
                heart2.SetActive(true);
                heart3.SetActive(true);
                nullHeart1.SetActive(true);
                nullHeart2.SetActive(false);
                nullHeart3.SetActive(false);
                break;
            case 1:
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(true);
                nullHeart1.SetActive(true);
                nullHeart2.SetActive(true);
                nullHeart3.SetActive(false);
                break;
            case 0:
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
                nullHeart1.SetActive(true);
                nullHeart2.SetActive(true);
                nullHeart3.SetActive(true);
                break;
        }
    }

    public void OnEvent(EventType eventType, Component Sender, object Param = null)
    {
        if (eventType == EventType.OnHit)
        {
            ChangeHP();
        }
        else if (eventType == EventType.OnHeal)
        {
            ChangeHP();
        }
    }
}
