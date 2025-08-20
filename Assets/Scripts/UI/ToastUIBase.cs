using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastUIBase : ToastUI
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshPro titleText;
    [SerializeField] TextMeshPro decText;
    ToastUIData toastUIData;

    [SerializeField] Button button1;

    private void Awake()
    {
        base.Awake();
        buttons[button1.name].onClick.AddListener(() => { Back(); });
    }

    void Back() 
    {
        Debug.Log("Back button clicked");
    }

    /*private void OnEnable()
    {
        if (this.isActiveAndEnabled)
        {
            Set(toastUIData, titleText, decText, iconImage);
        }
    }
    private void OnDisable() 
    { 
        Clear();  // 풀 반환 직전/후에도 안전
    }

    public void Clear()
    {
        toastUIData = null;
        if (titleText) titleText.text = string.Empty;
        if (decText) decText.text = string.Empty;
        if (iconImage) { iconImage.sprite = null; iconImage.enabled = false; }
    }*/

   
}
