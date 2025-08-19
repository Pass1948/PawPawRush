using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // 현재 누적된 점수
    private int baseScore = 0;

    // 점수 배율 (기본: 1배, 아이템 획득 시 2배 등)
    private float scoreMultiplier = 1f;

    // 인스턴스 초기화 (싱글톤 구성)
    private void Awake()
    {
        if (Instance == null)
            Instance = this;        // 첫 번째 ScoreManager가 자신을 인스턴스로 설정
        else
            Destroy(gameObject);    // 중복된 ScoreManager는 제거
    }

    /// 점수를 추가합니다. 배수(multiplier)를 적용해서 최종 점수를 계산합니다.
    public void AddScore(int amount)
    {
        baseScore += Mathf.RoundToInt(amount * scoreMultiplier);
        Debug.Log("Score: " + baseScore);
    }

    /// 점수 배수를 설정합니다. (예: 2배 점수 아이템 사용 시)
    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }
}
