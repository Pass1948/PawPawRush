using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // 장애물 프리팹 목록
    public List<GameObject> obstaclePrefabs;

    // 스폰 포인트
    public List<Transform> spawnPoints;

    public void SpawnObstacleOnPlatform()
    {
        Transform selectedSpawnPoint = spawnPoints[0];

        // 스폰할 장애물 프리팹 하나를 무작위로 선택
        int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
        GameObject selectedObstacle = obstaclePrefabs[randomObstacleIndex];

        // 선택된 스폰 포인트에 장애물을 생성
        GameObject newObstacle = Instantiate(selectedObstacle, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        
        // 장애물을 스폰 포인트의 자식으로 설정
        newObstacle.transform.SetParent(selectedSpawnPoint);
        
        // 위치와 회전 초기화
        newObstacle.transform.localPosition = Vector3.zero;
        newObstacle.transform.localRotation = Quaternion.identity;
    }
}
