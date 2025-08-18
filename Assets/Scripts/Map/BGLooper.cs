using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
    // ObstacleSpawner 스크립트를 참조하여 맵 생성을 요청합니다.
    public ObstacleSpawner obstacleSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            obstacleSpawner.SpawnObstacle();
            Destroy(other.gameObject);
        }
    }
}
