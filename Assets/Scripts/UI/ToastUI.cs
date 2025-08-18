using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastUI : BaseUI
{
    private readonly Queue<ToastUIData> pending = new();
    protected override void Awake()
    {
        base.Awake();
    }



}
