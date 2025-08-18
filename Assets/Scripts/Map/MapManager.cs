using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 스폰할 발판 프리팹 목록
    public List<GameObject> platformPrefabs;

    // 현재 활성화된 발판들을 관리하는 리스트
    private List<GameObject> activePlatforms = new List<GameObject>();

    // 스폰 지점
    public Transform spawnPoint;

    // 발판 삭제 지점
    public Transform destroyPoint;

    private void Start()
    {
        // 게임 시작 시, 초기 발판 생성
        for (int i = 0; i < 10; i++) // 초기 발판 10개 생성
        {
            SpawnNewPlatform();
        }
    }

    private void Update()
    {
        // 플레이어가 발판 끝에 도달하면 새로운 발판 생성
        if (activePlatforms.Count > 0 && activePlatforms[activePlatforms.Count - 1].transform.position.z < spawnPoint.position.z)
        {
            SpawnNewPlatform();
        }

        // 플레이어 뒤로 멀어진 발판 제거
        if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.z < destroyPoint.position.z)
        {
            DestroyOldestPlatform();
        }
    }

    private void SpawnNewPlatform()
    {
        // 발판 프리팹 목록에서 무작위로 선택
        int randomIndex = Random.Range(0, platformPrefabs.Count);
        GameObject selectedPlatform = platformPrefabs[randomIndex];
        
        // 마지막 발판 위치 계산
        Vector3 spawnPosition = Vector3.zero;
        if (activePlatforms.Count > 0)
        {
            GameObject lastPlatform = activePlatforms[activePlatforms.Count - 1];
            // 마지막 발판의 길이를 고려하여 정확하게 이어붙임
            spawnPosition = lastPlatform.transform.position + new Vector3(0, 0, GetPlatformLength(lastPlatform));
        } else
        {
            // 첫 발판의 위치는 0,0,0
            spawnPosition = Vector3.zero;
        }

        // 새로운 발판 생성
        GameObject newPlatform = Instantiate(selectedPlatform, spawnPosition, Quaternion.identity);
        activePlatforms.Add(newPlatform);
        
        // 생성된 발판에 장애물 스포너 로직을 호출 (아래 ObstacleSpawner.cs와 연결)
        ObstacleSpawner spawner = newPlatform.GetComponent<ObstacleSpawner>();
        if (spawner != null)
        {
            spawner.SpawnObstacleOnPlatform();
        }
    }

    private void DestroyOldestPlatform()
    {
        GameObject oldestPlatform = activePlatforms[0];
        activePlatforms.RemoveAt(0);
        Destroy(oldestPlatform);
    }

    // 발판의 길이를 계산하는 헬퍼 함수
    private float GetPlatformLength(GameObject platform)
    {
        // 발판의 BoxCollider를 사용하여 길이(z축)를 얻음
        // 발판에 BoxCollider가 반드시 있어야 함
        BoxCollider collider = platform.GetComponent<BoxCollider>();
        if (collider != null)
        {
            return collider.size.z;
        }
        return 0f; // 콜라이더가 없으면 길이 0 반환
    }
}