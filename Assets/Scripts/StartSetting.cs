using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetting : MonoBehaviour
{
    // 씬 라이트 설정하자!
    private void Start()
    {
        GameManager.UI.ShowWindowUI<StartSceneUI>("UI/StartSceneUI");
    }
}
