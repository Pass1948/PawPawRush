using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider volumeSlider2;
    [SerializeField] Button moveTitleButton;
    [SerializeField] Button leaveGameButton;
    [SerializeField] Button closeButton;

    protected override void Awake()
    {
        base.Awake();
        buttons[moveTitleButton.name].onClick.AddListener(() => { GameManager.Scene.LoadScene("TitleScene"); });
        buttons[leaveGameButton.name].onClick.AddListener(() => { LeaveGameButton(); });
        buttons[closeButton.name].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });

        volumeSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        volumeSlider.value = GameManager.Sound.GetVolume(VolumeType.BGM);

        volumeSlider2.onValueChanged.AddListener(OnSFXSliderChanged);
        volumeSlider2.value = GameManager.Sound.GetVolume(VolumeType.SFX);
    }
    private void OnBgmSliderChanged(float value)
    {
        GameManager.Sound.SetVolume(VolumeType.BGM, value);
    }
    private void OnSFXSliderChanged(float value)
    {
        GameManager.Sound.SetVolume(VolumeType.SFX, value);
    }

    public void LeaveGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
