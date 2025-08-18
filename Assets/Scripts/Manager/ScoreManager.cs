using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Á¡¼ö Áõ°¡
            ScoreManager.Instance.AddScore(1);

            // ÄÚÀÎ ÆÄ±«
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        // ½Ì±ÛÅæ ÆÐÅÏ
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        scoreText.text = score.ToString("D5");
    }

    public int GetScore()
    {
        return score;
    }
}

