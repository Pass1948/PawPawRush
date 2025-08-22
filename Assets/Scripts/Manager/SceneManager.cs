using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour // 씬 전환을 관리하는 매니저 클래스
{
    private string currentSceneName; // 현재 씬 이름

    public float progress { get; protected set; }
    public void LoadScene(string sceneName) 
        // 지정한 이름의 씬을 비동기로 로드
        // <param name="sceneName">로드할 씬 이름</param>
    {
        Debug.Log($"{sceneName}");
        StartCoroutine(LoadingCoroutine(sceneName));
    }

    IEnumerator LoadingCoroutine(string sceneName) 
        // 씬 로딩 코루틴. 로딩 전후로 시간 정지 및 재개 처리
        // <param name="sceneName">로드할 씬 이름</param>
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f; // 씬 로딩 전 시간 정지
        // 비동기로 씬을 로드

        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            yield return null;
        }
        LoadAsync(sceneName);
        // 씬 로딩 후 시간 재개
        while (progress < 1f)
        {
            yield return null;
        }
        Time.timeScale = 1f;
        Debug.Log($"로딩 완료");
        yield return new WaitForSeconds(0.5f);
    }
    private void LoadAsync(string sceneName)
    {
        currentSceneName = UnitySceneManager.GetActiveScene().name;
        StartCoroutine(LoadingRoutine(currentSceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        progress = 0.5f;
        SceneRecreated();
        progress = 0.8f;

        if(sceneName == "TitleScene")
        {
            GameManager.UI.ShowWindowUI<StartSceneUI>("UI/StartSceneUI");
            GameManager.Sound.PlayBGM("StartBGM");
        }
        else if (sceneName == "InGameScene")
        {

            GameManager.UI.ShowWindowUI<InGameSceneUI>("UI/InGameSceneUI");
            GameManager.Sound.PlayBGM("InGameBGM");
            MapManager.Instance.ReStart();
        }

        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("Scene준비완료");
        progress = 1.0f;
        yield return new WaitForSecondsRealtime(0.5f);
    }
    public void SceneRecreated()
    {
        GameManager.Pool.Recreated();
        GameManager.UI.Recreated();
    }

    public void SceneClear()
    {
        GameManager.Pool.Clear();
    }
}
