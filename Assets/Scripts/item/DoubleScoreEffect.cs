using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScoreEffect : MonoBehaviour
{
    [SerializeField] private float effectDuration = 5f;  // 2배 점수 지속 시간

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ActivateDoubleScore());
            Destroy(gameObject);  // 아이템 먹으면 사라짐
        }
    }

    private IEnumerator ActivateDoubleScore()
    {
        GameManager.Score.SetScoreMultiplier(2f);  // 2배 설정
        yield return new WaitForSeconds(effectDuration);
        GameManager.Score.SetScoreMultiplier(1f);  // 효과 종료 시 원래대로
    }
}

