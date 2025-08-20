using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastUI : BaseUI
{
    protected override void Awake()
    {
        base.Awake();
    }
    public void Set(ToastUIData data, TMP_Text title, TMP_Text desc, Image icon)
    {
        if (data == null)
        {
            Debug.LogWarning($"{nameof(ToastUIBase)}: data is null.", this);
            return;
        }

        if (title!=null) title.text = string.IsNullOrEmpty(data.title) ? "" : data.title;
        if (desc!=null) desc.text = string.IsNullOrEmpty(data.desc) ? "" : data.desc;

        if (icon!= null)
        {
            icon.sprite = data.icon;
            icon.enabled = (data.icon != null);
        }
    }
}
