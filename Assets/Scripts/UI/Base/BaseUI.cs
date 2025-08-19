using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    protected Dictionary<string, RectTransform> rects;
    protected Dictionary<string, Button> buttons;
    protected Dictionary<string, TMP_Text> texts;
    protected virtual void Awake()
    {
        BindChildren();
    }

    protected virtual void OnDisable() { }

    private void BindChildren()
    {
        rects = new Dictionary<string, RectTransform>();
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();

        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);

        foreach (var rect in children)
        {
            string key = GetPath(rect);

            if (!rects.ContainsKey(key)) rects.Add(key, rect);

            var btn = rect.GetComponent<Button>();
            if (btn != null && !buttons.ContainsKey(key)) 
                buttons.Add(key, btn);

            var tmp = rect.GetComponent<TMP_Text>();
            if (tmp != null && !texts.ContainsKey(key)) 
                texts.Add(key, tmp);
        }
    }
    private static string GetPath(Transform t)
    {
        var path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = $"{t.name}/{path}";
        }
        return path;
    }
}

