using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour // 씬 전환을 관리하는 매니저 클래스
{
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
        // 씬 로딩 후 시간 재개
        Time.timeScale = 1f;
        Debug.Log($"로딩 완료");
        yield return new WaitForSeconds(0.5f);
    }
}
