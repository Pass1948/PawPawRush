using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private int scoreAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Score.AddScore(scoreAmount);
            GameManager.Sound.PlaySFX("CoinSFX"); // 코인 획득 사운드 재생
            // TODO: 오브젝트 풀링
            Destroy(gameObject); // 아이템은 사용 후 제거
        }
    }
}
