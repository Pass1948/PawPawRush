using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; 
    public Transform obstacleParent; 
    public int numberOfInitialObstacles = 4; // 시작할 때 미리 생성할 개수
    [HideInInspector] public float obstacleLength;

    private float _zSpawnPosition = 0f;

    private void Awake()
    {
        // 발판 전체 길이 자동 계산 (자식 포함)
        Renderer[] renderers = obstaclePrefabs[0].GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer rend in renderers)
            bounds.Encapsulate(rend.bounds);

        obstacleLength = bounds.size.z;
        Debug.Log("자동 계산된 발판 길이: " + obstacleLength);
    }

    private void Start()
    {
        SpawnInitialObstacles();
    }

    public void SpawnInitialObstacles()
    {
        for (int i = 0; i < numberOfInitialObstacles; i++)
        {
            SpawnObstacle();
        }
    }

    public void SpawnObstacle()
    {
        // Pivot 중앙일 경우 보정
        Vector3 spawnPos = new Vector3(0, 0, _zSpawnPosition + obstacleLength / 2f);

        GameObject newObstacle = Instantiate(
            obstaclePrefabs[0],
            spawnPos,
            Quaternion.identity,
            obstacleParent
        );

        _zSpawnPosition += obstacleLength; // 다음 발판 위치 갱신
    }
}