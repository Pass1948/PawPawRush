using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // ��ֹ� ������ �迭
    public float obstacleLength = 30f; // �� �������� ����
    public int numberOfObstacles = 3; // ó�� ������ �� �������� ����

    private float zSpawn = 0f; // �

    void Start()
    {
        // ���� ���� �� �ʱ� �� ����
        for (int i = 0; i < numberOfObstacles; i++)
        {
            SpawnObstacle(Random.Range(0, obstaclePrefabs.Length));
        }
    }

    // �� ���� �Լ�
    public void SpawnObstacle(int obstacleIndex)
    {
        GameObject newObstacle = Instantiate(obstaclePrefabs[obstacleIndex], transform.forward * zSpawn, transform.rotation);
        zSpawn += obstacleLength;
    }
}