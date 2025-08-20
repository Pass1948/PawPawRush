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

    private void Awake()
    {
        base.Awake();
        Set(toastUIData, titleText, decText, iconImage)
    }
    

}
