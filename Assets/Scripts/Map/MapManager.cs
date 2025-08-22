using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("MapInfo")]
    // 발판 프리팹 목록
    [SerializeField] private List<GameObject> platformPrefabs;

    // 현재 활성화된 발판들을 관리하는 리스트
    private List<GameObject> activePlatforms = new List<GameObject>();

    [SerializeField] private MapMovement mapMovement;
    public MapMovement MapMovement { get { return mapMovement; } set { mapMovement = value; } }
    [SerializeField] private float origMapMovementSpeed;
    public float OrigMapMovementSpeed { get { return origMapMovementSpeed; } }

    // 스폰 지점
    [SerializeField] private Transform spawnPoint;

    // 발판 삭제 지점
    [SerializeField] private Transform destroyPoint;

    [Header("Sound")]
    [SerializeField] private AudioClip countDownSound; // 카운트다운 사운드
    private AudioSource audioSource;

    // 카운트 다운 시간
    public float countdownDuration = 5.0f;

    private bool isGamePaused = false;
    private bool isGameRunning = false;

    // 코루틴
    private Coroutine playerPreparationCoroutine;
    private Coroutine speedBoostCoroutine;

    // 이벤트
    public static event Action OnMapMovementStarted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReStart() 
    {
        PrepareInitialMap();
        StartGame();
    }

    private void Update()
    {
        if (isGameRunning && !isGamePaused) // 게임이 실행 중이고 일시 정지 상태가 아닐 때
        {
            if (mapMovement != null)
            {
                mapMovement.Move();
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
        if (mapMovement == null)
        {
            Debug.LogError("MapMovement가 MapManager에 연결되지 않았습니다!");
            return;
        }

        // 원래 속도를 저장 후 현재 속도 0으로 설정하여 맵을 정지시킴
        origMapMovementSpeed = mapMovement.movementSpeed;
        mapMovement.movementSpeed = 0f;

        // 초기 발판들 생성
        // 초기 설정 수를 변수로 뺴두면 좋음
        // 로직에 들어가는 수 등의 데이터는 변수로
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

    // 폴 매니저를 같이 쓰면 좋을듯
    private void SpawnNewPlatform(bool spawnObstacles)
    {
        // 발판 프리팹 목록에서 무작위로 선택
        int randomIndex = UnityEngine.Random.Range(0, platformPrefabs.Count);
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

    // 시퀸스 개념 자체는 좋다.
    // 모듈화 하면 좋음.
    // 특히 지금처럼 씬 시작전체 필요한 일을 순서대로 처리할 수 있음 -> API 통신있을때 유용
    
    // 맵 매니저라지만 - 해당 씬을 총괄하는 초기화로 보임
    private IEnumerator GameStartSequence()
    {
        // 플레이어 캐릭터가 씬에 로드될 때까지 대기
        while (GameManager.Player.PlayerCharacter == null)
        {
            yield return null;
        }

        PlayerController playerController = GameManager.Player.PlayerCharacter.PlayerController;
        playerPreparationCoroutine = StartCoroutine(playerController.PrepareForGameStart(countdownDuration));

        // 카운트다운 효과음 재생
        audioSource.PlayOneShot(countDownSound);

        float currentTime = countdownDuration;
        while (currentTime > 0)
        {
            // 남은 시간을 계산
            int displayTime = Mathf.CeilToInt(currentTime);
            Debug.Log($"남은 시간: {displayTime}초");

            // TODO: 카운트다운 UI 표시
            // UIManager에서 계산된 시간을 텍스트로 업데이트
            // 예) uiManager.UpdateCountdownText(displayTime.ToString());

            currentTime -= Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // TODO: "START!" UI 메시지 표시
        // 예) uiManager.UpdateCountdownText("START!");

        Debug.Log("플레이어 준비 시퀀스 시작...");
        // 플레이어 준비 동작이 완전히 끝날 때까지 대기
        yield return playerPreparationCoroutine;
        Debug.Log("플레이어 준비 완료. 달리기 시작됨.");

        // 1초 후 카운트다운 UI 숨김 명령 또는 바로 사라짐?
        yield return new WaitForSeconds(1.0f);
        //uiManager.HideCountdownDisplay();

        isGameRunning = true;
        isGamePaused = false;

        // 이동 속도를 원래 속도로 설정
        if (mapMovement != null)
        {
            mapMovement.movementSpeed = origMapMovementSpeed;
        }

        // 좋은 이벤트 개념
        // 해당 구조에 좋은 개념이 많이 들어가 있긴한데 짬뽕...
        // 일관성은 좀 부족한다
        OnMapMovementStarted?.Invoke(); // 맵 이동 시작 이벤트 발생

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
        if (mapMovement != null)
        {
            mapMovement.movementSpeed = 0;
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
            if (mapMovement != null)
            {
                mapMovement.movementSpeed = 0;
            }
        }
        else
        {
            Debug.Log("게임이 다시 시작되었습니다.");
            // 맵 이동 다시
            if (mapMovement != null)
            {
                mapMovement.movementSpeed = origMapMovementSpeed;
            }
        }
    }

    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        // 게임이 실행 중이 아닐 때
        if (!isGameRunning || isGamePaused)
        {
            return;
        }

        // 만약 이전에 실행되던 스피드 부스트 코루틴이 있다면 중지
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        // 새로운 스피드 부스트 코루틴을 시작하고 변수에 저장
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(boostAmount, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float boostAmount, float duration)
    {
        Debug.Log($"스피드 부스트 활성화! 속도 {boostAmount}배, 지속 시간: {duration}초");

        // 맵의 속도를 원래 속도에 부스트 양을 곱한 값으로 설정
        mapMovement.movementSpeed = origMapMovementSpeed * boostAmount;

        // 지정된 지속 시간만큼 대기
        yield return new WaitForSeconds(duration);

        Debug.Log("스피드 부스트 종료. 원래 속도로 복귀.");

        // 맵의 속도를 원래 속도로 복원
        // isGamePaused 상태가 아닐 때만 복원 -> 일시정지 로직과 충돌 방지
        if (!isGamePaused)
        {
            mapMovement.movementSpeed = origMapMovementSpeed;
        }

        // 코루틴 끝 -> null로 초기화
        speedBoostCoroutine = null;
    }
}