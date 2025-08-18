using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// UI 오브젝트의 자식 컴포넌트(Button, Text 등)를 자동으로 바인딩하는 기본 UI 클래스

    public class BaseUI : MonoBehaviour
    {
        protected Dictionary<string, RectTransform> rectTransform; // 자식 RectTransform을 이름으로 관리하는 딕셔너리
        protected Dictionary<string, Button> buttons; // 자식 Button을 이름으로 관리하는 딕셔너리
        protected Dictionary<string, TMP_Text> texts; // 자식 TMP_Text를 이름으로 관리하는 딕셔너리
        protected virtual void Awake() // 오브젝트가 생성될 때 자식 컴포넌트 바인딩
        {
            BindChildren();
        }
        
        protected virtual void OnDisable() {} // 오브젝트가 비활성화될 때 호출 (추후 확장용)

        private void BindChildren()
            // 자식 RectTransform, Button, TMP_Text를 이름 기준으로 딕셔너리에 바인딩

        {
            rectTransform = new Dictionary<string, RectTransform>();
            buttons = new Dictionary<string, Button>();
            texts = new Dictionary<string, TMP_Text>();

            RectTransform[] children = GetComponentsInChildren<RectTransform>();
            foreach (RectTransform child in children)
            {
                string key = child.gameObject.name;

                if (rectTransform.ContainsKey(key))
                    continue;

                rectTransform.Add(key, child);

                Button button = child.GetComponent<Button>();
                if (button != null)
                    buttons.Add(key, button);

                TMP_Text text = child.GetComponent<TMP_Text>();
                if (text != null)
                    texts.Add(key, text);
            }
        }
    }

