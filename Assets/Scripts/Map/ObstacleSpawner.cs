using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
    // 장애물 프리팹 목록
    public List<GameObject> obstaclePrefabs;
    // 장애물 스폰 포인트
    public List<Transform> obstacleSpawnPoints;

    // 아이템 프리팹 목록
    public List<GameObject> itemPrefabs;
    
    // 아이템이 생성될 확률
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.5f;
    
    public void SpawnObstacleOnPlatform()
    {
        if (obstacleSpawnPoints.Count > 0)
        {
            Transform selectedSpawnPoint = obstacleSpawnPoints[0];

            // 스폰할 장애물 프리팹 하나를 무작위로 선택
            int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
            GameObject selectedObstacle = obstaclePrefabs[randomObstacleIndex];

            // 선택된 스폰 포인트에 장애물을 생성
            GameObject newObstacle =
                Instantiate(selectedObstacle, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

            // 장애물을 스폰 포인트의 자식으로 설정
            newObstacle.transform.SetParent(selectedSpawnPoint);

            // 위치와 회전 초기화
            newObstacle.transform.localPosition = Vector3.zero;
            newObstacle.transform.localRotation = Quaternion.identity;
        }
        
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        List<Transform> itemSpawnPoints = new List<Transform>();
        
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("SpawnItem"))
            {
                itemSpawnPoints.Add(child);
            }
        }
        
        if (itemPrefabs.Count > 0 && itemSpawnPoints.Count > 0)
        {
            // 모든 스폰 포인트를 순회하며 각각의 위치에 대해 확률 검사
            foreach (Transform spawnPoint in itemSpawnPoints)
            {
                if (Random.value < itemSpawnChance)
                {
                    // 스폰될 아이템 프리팹을 무작위로 선택
                    int randomItemIndex = Random.Range(0, itemPrefabs.Count);
                    GameObject selectedItem = itemPrefabs[randomItemIndex];

                    // 선택된 위치에 아이템을 생성
                    GameObject newItem = Instantiate(selectedItem, spawnPoint.position, spawnPoint.rotation);
                    
                    // 생성된 아이템을 스폰 포인트의 자식으로 설정
                    newItem.transform.SetParent(spawnPoint.transform);
                }
            }
        }
    }
}
