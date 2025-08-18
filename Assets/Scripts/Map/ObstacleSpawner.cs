using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // 스폰할 장애물 프리팹 목록
    public List<GameObject> obstaclePrefabs;

    // 장애물을 생성할 빈 게임 오브젝트(스폰 포인트)
    public List<Transform> spawnPoints;

    public void SpawnObstacleOnPlatform()
    {
        // 각 스폰 포인트에 무작위로 장애물 생성
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            // 20% 확률로 장애물 생성
            if (Random.Range(0f, 1f) < 0.2f)
            {
                // 무작위 장애물 선택
                int randomIndex = Random.Range(0, obstaclePrefabs.Count);
                GameObject selectedObstacle = obstaclePrefabs[randomIndex];

                // 장애물 생성 및 부모 설정
                GameObject newObstacle = Instantiate(selectedObstacle, spawnPoints[i].position, Quaternion.identity);
                newObstacle.transform.SetParent(spawnPoints[i]);
            }
        }
    }
}