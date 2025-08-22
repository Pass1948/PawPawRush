using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScoreEffect : MonoBehaviour
{
    [SerializeField] private float effectDuration = 5f;  // 2배 점수 지속 시간
    // 오브젝트가 재활용되지 않아서 해당 플래스가 유의미할지 모르겠습니다
    bool isFirst = true;  // 첫 활성화 여부

    // 기능상 비슷한 구조라 아이템들을 하나의 베이스 클래스로 묶어서 관리하는 것을 추천
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFirst)
            {
                isFirst = false;
                GameManager.Event.PostNotification(EventType.AchievementUnlocked, this, AchievementId.FistDouble);
            }
            StartCoroutine(ActivateDoubleScore());
            Destroy(gameObject);  // 아이템 먹으면 사라짐
        }
    }
    
    private IEnumerator ActivateDoubleScore()
    {
        GameManager.Score.SetScoreMultiplier(2f);  // 2배 설정
        yield return new WaitForSeconds(effectDuration);
        // 원복이 되나요??
        // 부스트 처럼 활용 권장
        GameManager.Score.SetScoreMultiplier(1f);  // 효과 종료 시 원래대로
    }
}

