
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastUIBase : ToastUI
{
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text decText;
    ToastUIData toastUIData;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameManager.Sound.PlaySFX("ToastSFX");
        if (this.isActiveAndEnabled)
        {
            Set(toastUIData, titleText, decText, iconImage);
        }
    }
    private void OnDisable() 
    { 
        Clear();  // 풀 반환 직전/후에도 안전
    }
    public void SetData(ToastUIData data)
    {
        toastUIData = data;
        if (isActiveAndEnabled)
            Set(toastUIData, titleText, decText, iconImage);
    }

    public void Clear()
    {
        toastUIData = null;
        if (titleText) titleText.text = string.Empty;
        if (decText) decText.text = string.Empty;
        if (iconImage) { iconImage.sprite = null; iconImage.enabled = false; }
    }
}
