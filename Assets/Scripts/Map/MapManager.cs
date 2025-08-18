using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 맵 이동 스크립트와 맵 생성 스크립트를 연결합니다.
    public MapMovement mapMovement;
    public ObstacleSpawner obstacleSpawner;

    void Start()
    {
        // 초기 맵 만들기
        obstacleSpawner.SpawnInitialObstacles();
        
    }
    
    void Update()
    {
        // 맵 움직임
        mapMovement.MoveMap();
    }
}