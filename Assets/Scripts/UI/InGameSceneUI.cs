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
    [SerializeField] Image heart1;
    [SerializeField] Image heart2;
    [SerializeField] Image heart3;
    [SerializeField] Image nullHeart1;
    [SerializeField] Image nullHeart2;
    [SerializeField] Image nullHeart3;

    // 점수
    [SerializeField] TMP_Text scoreText;
    float countdownStart = 5;

    protected override void Awake()
    {
        base.Awake();
        buttons[SetBotton.name].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<SettingUI>("UI/SetUI"); });
    }
    private void OnEnable()
    {
        GameManager.Score.BaseScore = 0;
        GameManager.Event.AddListener(EventType.OnHeal, this);
        GameManager.Event.AddListener(EventType.OnHit, this);
    }
    private void Update()
    {
        scoreText.text = GameManager.Score.BaseScore.ToString();
        StartCountdow();
    }

    private void OnDisable()
    {
        GameManager.Score.BaseScore = 0;
        if (GameManager.Instance == null) return;
        GameManager.Event.RemoveListener(EventType.OnHeal, this);
        GameManager.Event.RemoveListener(EventType.OnHit, this);
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
        if (GameManager.Player.PlayerCharacter.PlayerCondition.CurrentLife == 3)
        {
            heart1.gameObject.SetActive(true);
            heart2.gameObject.SetActive(true);
            heart3.gameObject.SetActive(true);
            nullHeart1.gameObject.SetActive(false);
            nullHeart2.gameObject.SetActive(false);
            nullHeart3.gameObject.SetActive(false);
        }
       if(GameManager.Player.PlayerCharacter.PlayerCondition.CurrentLife == 2)
        {
            heart1.gameObject.SetActive(false);
            heart2.gameObject.SetActive(true);
            heart3.gameObject.SetActive(true);
            nullHeart1.gameObject.SetActive(true);
            nullHeart2.gameObject.SetActive(false);
            nullHeart3.gameObject.SetActive(false);
        }
        if(GameManager.Player.PlayerCharacter.PlayerCondition.CurrentLife == 1)
        {
            heart1.gameObject.SetActive(false);
            heart2.gameObject.SetActive(false);
            heart3.gameObject.SetActive(true);
            nullHeart1.gameObject.SetActive(true);
            nullHeart2.gameObject.SetActive(true);
            nullHeart3.gameObject.SetActive(false);
        }
        if (GameManager.Player.PlayerCharacter.PlayerCondition.CurrentLife == 0)
        {
            heart1.gameObject.SetActive(false);
            heart2.gameObject.SetActive(false);
            heart3.gameObject.SetActive(false);
            nullHeart1.gameObject.SetActive(true);
            nullHeart2.gameObject.SetActive(true);
            nullHeart3.gameObject.SetActive(true);
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
