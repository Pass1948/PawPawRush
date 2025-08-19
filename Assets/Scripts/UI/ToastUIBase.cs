using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastUIBase : ToastUI
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI decText;

    private void Awake()
    {
        base.Awake();
    }
}
