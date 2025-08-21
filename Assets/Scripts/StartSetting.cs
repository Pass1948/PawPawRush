using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetting : MonoBehaviour
{
    private void Start()
    {
        GameManager.UI.ShowWindowUI<WindowUI>("UI/StartSceneUI");
    }
}
