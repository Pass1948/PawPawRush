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

    private static MapManager mapManager;
    public static MapManager Map => mapManager;

    private static EventManager eventManager;
    public static EventManager Event => eventManager;



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

        GameObject mapObj = new GameObject("MapManager");
        mapObj.transform.SetParent(transform, false);
        mapManager = mapObj.AddComponent<MapManager>();

        GameObject eventObj = new GameObject("EventManager");
        eventObj.transform.SetParent(transform, false);
        eventManager = eventObj.AddComponent<EventManager>();
    }

}
