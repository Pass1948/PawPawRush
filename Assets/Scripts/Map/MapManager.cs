using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    // 발판 프리팹 목록
    public List<GameObject> platformPrefabs;

    // 현재 활성화된 발판들을 관리하는 리스트
    private List<GameObject> activePlatforms = new List<GameObject>();

    public MapMovement MapMovement;
    public float OrigMapMovementSpeed;

    // 스폰 지점
    public Transform spawnPoint;

    // 발판 삭제 지점
    public Transform destroyPoint;

    // 카운트 다운 시간
    public float countdownDuration = 5.0f;

    private bool isGamePaused = false;
    private bool isGameRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PrepareInitialMap();

        // Temp: 게임 시작은 UI 버튼 등 다른 곳에서 StartGame()을 호출하여 시작
        StartGame();
    }

    private void Update()
    {
        if (isGameRunning && !isGamePaused) // 게임이 실행 중이고 일시 정지 상태가 아닐 때
        {
            if (MapMovement != null)
            {
                MapMovement.Move();
            }
            else
            {
                Debug.LogWarning("MapMovement가 설정되지 않았습니다.");
            }

            // 새로운 발판 생성
            if (activePlatforms.Count > 0 && activePlatforms[activePlatforms.Count - 1].transform.position.z < spawnPoint.position.z)
            {
                SpawnNewPlatform(true);
            }

            // 플레이어 뒤로 멀어진 발판 제거
            if (activePlatforms.Count > 0 && activePlatforms[0].transform.position.z < destroyPoint.position.z)
            {
                DestroyOldestPlatform();
            }
        }
    }

    // 움직이지 않는 초기 맵 생성
    private void PrepareInitialMap()
    {
        // MapMovement 컴포넌트 연결되어 있는지 확인
        if (MapMovement == null)
        {
            Debug.LogError("MapMovement가 MapManager에 연결되지 않았습니다!");
            return;
        }

        // 원래 속도를 저장 후 현재 속도 0으로 설정하여 맵을 정지시킴
        OrigMapMovementSpeed = MapMovement.movementSpeed;
        MapMovement.movementSpeed = 0f;

        // 초기 발판들 생성
        for (int i = 0; i < 2; i++)
        {
            SpawnNewPlatform(false);
        }
        for (int i = 0; i < 8; i++)
        {
            SpawnNewPlatform(true);
        }

        Debug.Log("초기 맵 생성 완료. 게임 시작 대기 중...");
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

    //플랫폼 길이 계산
    private float GetPlatformLength(GameObject platform) 
    {
        BoxCollider collider = platform.GetComponent<BoxCollider>();
        if (collider != null)
        {
            return collider.size.z;
        }
        return 0f;
    }
    
    public void StartGame()
    {
        if (isGameRunning)
        {
            Debug.Log("게임이 이미 실행 중입니다.");
            return;
        }

        StartCoroutine(GameStartSequence());
    }

    private IEnumerator GameStartSequence()
    {
        // 플레이어 캐릭터가 씬에 로드될 때까지 대기
        while (GameManager.Player.PlayerCharacter == null)
        {
            yield return null;
        }

        PlayerController playerController = GameManager.Player.PlayerCharacter.PlayerController;

        // TODO: 카운트다운 UI 표시

        // 플레이어 준비 동작 시작
        Debug.Log("플레이어 준비 시퀀스 시작...");
        yield return StartCoroutine(playerController.PrepareForGameStart(countdownDuration));
        Debug.Log("플레이어 준비 완료. 달리기 시작됨.");

        isGameRunning = true;
        isGamePaused = false;

        // 이동 속도를 원래 속도로 설정
        if (MapMovement != null)
        {
            MapMovement.movementSpeed = OrigMapMovementSpeed;
        }

        // TODO: "START!" UI 메시지 표시
        Debug.Log("맵 이동 시작. 게임 플레이 시작!");
    }

    public void EndGame()
    {
        if (!isGameRunning)
        {
            Debug.Log("게임이 이미 종료되었습니다.");
            return;
        }
        
        isGameRunning = false;
        isGamePaused = false;

        // 맵 멈춤
        if (MapMovement != null)
        {
            MapMovement.movementSpeed = 0;
        }
    }
    
    public void PauseGame()
    {
        if (!isGameRunning)
        {
            Debug.Log("게임이 실행 중이 아닙니다.");
            return;
        }

        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            // 맵 이동 멈춤
            if (MapMovement != null)
            {
                MapMovement.movementSpeed = 0;
            }
        }
        else
        {
            Debug.Log("게임이 다시 시작되었습니다.");
            // 맵 이동 다시
            if (MapMovement != null)
            {
                MapMovement.movementSpeed = OrigMapMovementSpeed;
            }
        }
    }
    
}