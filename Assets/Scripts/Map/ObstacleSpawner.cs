using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // 장애물 프리팹 목록
    public List<GameObject> obstaclePrefabs;
    // 스폰 포인트
    public List<Transform> obstacleSpawnPoints;

    // 아이템 프리팹 목록
    public List<GameObject> itemPrefabs;
    // 아이템 스폰 포인트 (Start에서 초기화)
    private GameObject[] itemSpawnPoints;
    // 아이템이 생성될 확률
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.5f;
    private void Awake()
    {
        itemSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnItem");
        Debug.Log("1. Awake() 실행: 'SpawnItem' 태그를 가진 오브젝트 " + itemSpawnPoints.Length + "개 찾음.");
    }
    private void Start()
    {
        // "SpawnItem" 태그를 가진 모든 오브젝트를 찾아 배열에 저장
        itemSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnItem");
    }
    
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

        if (itemPrefabs.Count > 0 && itemSpawnPoints != null && itemSpawnPoints.Length > 0)
        {
            Debug.Log("3. 아이템 스폰 조건 충족: Prefabs=" + itemPrefabs.Count + ", SpawnPoints=" + itemSpawnPoints.Length);
            // 모든 아이템 스폰 포인트를 순회하며 개별적으로 확률 검사
            foreach (GameObject spawnPoint in itemSpawnPoints)
            {
                if (Random.value < itemSpawnChance)
                {
                    Debug.Log("4. 아이템 스폰 성공! 위치: " + spawnPoint.transform.position);

                    // 스폰될 아이템 프리팹을 무작위로 선택
                    int randomItemIndex = Random.Range(0, itemPrefabs.Count);
                    GameObject selectedItem = itemPrefabs[randomItemIndex];

                    // 아이템을 생성하고 변수에 저장
                    GameObject newItem = Instantiate(selectedItem, spawnPoint.transform.position, spawnPoint.transform.rotation);
            
                    // 생성된 아이템을 스폰 포인트의 자식으로 설정
                    newItem.transform.SetParent(spawnPoint.transform);
                }
            }
        }
        else
        {
            Debug.LogWarning("아이템 스폰 조건 미충족. 아래 사항들을 확인하세요.");
            if (itemPrefabs.Count == 0) Debug.LogWarning("    -> 'Item Prefabs' 리스트가 비어 있습니다.");
            if (itemSpawnPoints.Length == 0) Debug.LogWarning("    -> 'SpawnItem' 태그를 가진 오브젝트가 씬에 없거나, 발판이 생성된 이후에 찾지 못하고 있습니다.");
        }
    }
}
