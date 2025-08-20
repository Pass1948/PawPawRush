using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 스폰할 발판 프리팹 목록
    public List<GameObject> platformPrefabs;

    // 현재 활성화된 발판들을 관리하는 리스트
    private List<GameObject> activePlatforms = new List<GameObject>();

    public MapMovement MapMovement { get; set; }
    public float OrigMapMovementSpeed { get; set; }

    // 스폰 지점
    public Transform spawnPoint;

    // 발판 삭제 지점
    public Transform destroyPoint;

    private void Start()
    {
        OrigMapMovementSpeed = GameManager.Map.MapMovement.movementSpeed;

        // 첫 2개의 발판은 장애물 없이 생성
        for (int i = 0; i < 2; i++)
        {
            SpawnNewPlatform(false);
        }

        // 나머지 발판은 장애물을 포함하여 생성
        for (int i = 0; i < 8; i++)
        {
            SpawnNewPlatform(true);
        }
    }

    private void Update()
    {
        if (MapMovement != null)
        {
            MapMovement.Move();
        }
        
        // 새로운 발판 생성
        if (activePlatforms.Count > 0 && activePlatforms[activePlatforms.Count - 1].transform.position.z < spawnPoint.position.z)
        {
            SpawnNewPlatform(true); // 업데이트 중에는 항상 장애물 포함 생성
        }

        // 플레이어 뒤로 멀어진 발판 제거
        if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.z < destroyPoint.position.z)
        {
            DestroyOldestPlatform();
        }
    }

    private void SpawnNewPlatform(bool spawnObstacles)
    {
        // 발판 프리팹 목록에서 무작위로 선택
        int randomIndex = Random.Range(0, platformPrefabs.Count);
        GameObject selectedPlatform = platformPrefabs[randomIndex];
        
        // 마지막 발판 위치 계산
        Vector3 spawnPosition = Vector3.zero;
        if (activePlatforms.Count > 0)
        {
            GameObject lastPlatform = activePlatforms[activePlatforms.Count - 1];
            // 정확하게 이어붙임
            spawnPosition = lastPlatform.transform.position + new Vector3(0, 0, GetPlatformLength(lastPlatform));
        } else
        {
            // 첫 발판의 위치는 0,0,0
            spawnPosition = Vector3.zero;
        }

        // 새로운 발판 생성
        GameObject newPlatform = Instantiate(selectedPlatform, spawnPosition, Quaternion.identity, this.transform);
        activePlatforms.Add(newPlatform);
        
        // 장애물 스포너를 호출
        if (spawnObstacles)
        {
            ObstacleSpawner spawner = newPlatform.GetComponent<ObstacleSpawner>();
            if (spawner != null)
            {
                spawner.SpawnObstacleOnPlatform();
            }
        }
    }

    private void DestroyOldestPlatform()
    {
        GameObject oldestPlatform = activePlatforms[0];
        activePlatforms.RemoveAt(0);
        Destroy(oldestPlatform);
    }

    private float GetPlatformLength(GameObject platform)
    {
        BoxCollider collider = platform.GetComponent<BoxCollider>();
        if (collider != null)
        {
            return collider.size.z;
        }
        return 0f;
    }
}