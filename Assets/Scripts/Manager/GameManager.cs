using System.Resources;
using UnityEngine;

[DefaultExecutionOrder(-100)]       // GameManager가 다른 Script보다 먼저 호출되게 설정
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // Managers=========================
    private static ResourceManager resourceManager;
    public static ResourceManager Resource => resourceManager;

    private static PoolManager poolManager;
    public static PoolManager Pool => poolManager;

    private static SceneManager sceneManager;
    public static SceneManager Scene => sceneManager;

    private static UIManager uiManager;
    public static UIManager UI => uiManager;

    private static PlayerManager playerManager;
    public static PlayerManager Player => playerManager;

    private static EventManager eventManager;
    public static EventManager Event => eventManager;

    private static ScoreManager scoreManager;
    public static ScoreManager Score => scoreManager;

    private static AchievenmentManager achievenmentManager;
    public static AchievenmentManager Achievenment => achievenmentManager;

    private static SoundManager soundManager;
    public static SoundManager Sound=> soundManager;




    private void Awake()
    {
        if (instance != null) { Destroy(this); return; }
        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        // 자동화 코드 만들어두면 편할듯
        GameObject resourceObj = new GameObject("ResourceManager");
        resourceObj.transform.SetParent(transform, false);
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject("PoolManager");
        poolObj.transform.SetParent(transform, false);
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject sceneObj = new GameObject("SceneManager");
        sceneObj.transform.SetParent(transform, false);
        sceneManager = sceneObj.AddComponent<SceneManager>();

        GameObject uiObj = new GameObject("UIManager");
        uiObj.transform.SetParent(transform, false);
        uiManager = uiObj.AddComponent<UIManager>();

        GameObject playerObj = new GameObject("PlayerManager");
        playerObj.transform.SetParent(transform, false);
        playerManager = playerObj.AddComponent<PlayerManager>();

        GameObject eventObj = new GameObject("EventManager");
        eventObj.transform.SetParent(transform, false);
        eventManager = eventObj.AddComponent<EventManager>();

        GameObject scoreObj = new GameObject("ScoreManager");
        scoreObj.transform.SetParent(transform, false);
        scoreManager = scoreObj.AddComponent<ScoreManager>();

        GameObject achievenmentObj = new GameObject("AchievenmentManager");
        achievenmentObj.transform.SetParent(transform, false);
        achievenmentManager = achievenmentObj.AddComponent<AchievenmentManager>();

        GameObject soundObj = new GameObject("SoundManager");
        soundObj.transform.SetParent(transform, false);
        soundManager = soundObj.AddComponent<SoundManager>();

    }

}
