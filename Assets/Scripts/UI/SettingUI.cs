
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
        // 람다와 아닌것의 기준은??
        buttons[moveTitleButton.name].onClick.AddListener(() => { OnTitle(); });
        buttons[leaveGameButton.name].onClick.AddListener(LeaveGameButton);
        buttons[closeButton.name].onClick.AddListener(OnClose);

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

    void OnTitle() 
    {
        Time.timeScale = 1f;
        GameManager.UI.ClosePopUpUI();
        GameManager.Scene.LoadScene("TitleScene");
    }

    void OnClose()
    {
        Time.timeScale = 1f;
        GameManager.UI.ClosePopUpUI();
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
