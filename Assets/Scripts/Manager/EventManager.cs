using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum EventType
{
    TryJump10,               // 나는야 점프왕! : 점프 10회 시도
    FirstSubItem,           // 시간은 금이라고 친구 : 자석아이템 첫 획득
    Coin50,                     // 나는 부자가 될꺼야 : 코인 50개 획득
    LRMove100,            // 와리가리 : 좌/우 움직임 횟수 100번

}


public class EventManager : MonoBehaviour
{
    // 이벤트 리스너 리스트를 Dictionary를 통해 관리 EventType은 IN과 OUT으로 두가지 분류의 리스트로 관리
    private Dictionary<EventType, List<IEventListener>> listeners = new Dictionary<EventType, List<IEventListener>>();

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerSceneLoaded;
    }
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManagerSceneLoaded;
    }
    private void SceneManagerSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //씬이 바뀜에 따라 이벤트 의존성을 제거해준다.
        RefreshListeners();
    }

    /*    public void AddListener(EventType eventType, IEventListener listener)       // 이벤트 받는 역할
        {
            List<IEventListener> ListenList = null;

            if (listeners.TryGetValue(eventType, out ListenList))
            {
                //해당 이벤트 키값이 존재한다면, 이벤트를 추가해준다.
                ListenList.Add(listener);
                return;
            }
            ListenList = new List<IEventListener>();
            ListenList.Add(listener);
            listeners.Add(eventType, ListenList);

        }*/

    // 이벤트 받는 역할 (메소드 시험중)
    public void AddListener(EventType evt, IEventListener listener)
    {
        if (listener == null) return;

        if (!listeners.TryGetValue(evt, out var set))
        {
            set = new HashSet<IEventListener>();
            listeners.Add(evt, set);
        }
        set.Add(listener);
    }



    public void PostNotification(EventType eventType, Component Sender, object Param = null) // 이벤트 발생역할
    {
        List<IEventListener> ListenList = null;

        //이벤트 리스너(대기자)가 없으면 그냥 리턴.

        if (!listeners.TryGetValue(eventType, out ListenList))
            return;


        //모든 이벤트 리스너(대기자)에게 이벤트 전송.
        for (int i = 0; i < ListenList.Count; i++)
        {
            if (!ListenList[i].Equals(null)) //If object is not null, then send message via interfaces
                ListenList[i].OnEvent(eventType, Sender, Param);
        }
    }


    public void RemoveEvent(EventType eventType)        // 모든이벤트 삭제
    {
        listeners.Remove(eventType);
    }
    public void RemoveListener(EventType evt, IEventListener listener)
    {
        if (!listeners.TryGetValue(evt, out var set)) return;
        set.Remove(listener);
        if (set.Count == 0) listeners.Remove(evt);
    }
    private void RefreshListeners()     // Scene전환시 모든 이벤트 초기화
    {
        //임시 Dictionary 생성
        Dictionary<EventType, List<IEventListener>> TmpListeners = new Dictionary<EventType, List<IEventListener>>();

        //씬이 바뀜에 따라 리스너가 Null이 된 부분을 삭제해준다. 
        foreach (KeyValuePair<EventType, List<IEventListener>> Item in listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }
        //살아있는 리스너는 다시 넣어준다.
        listeners = TmpListeners;
    }

}
