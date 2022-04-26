using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// イベントの種類
/// </summary>
public enum Events
{
    Setup,
    StageStart,
    StageEnd,
    CameraShake,
    GetKonpeitou,
    BossBattleStart,
    BossBattleEnd,
    RebindWeaponAnimation,
    OnHUD,
    OffHUD
}

/// <summary>
/// イベントを管理するクラス
/// </summary>
public class EventManager : MonoBehaviour
{
    /// <summary> 各イベントを管理するDictionary </summary>
    Dictionary<Events, Action> m_eventDic = new Dictionary<Events, Action>();

    public static EventManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// イベントを登録する
    /// </summary>
    /// <param name="events"> イベントの種類 </param>
    /// <param name="action"> 追加する処理 </param>
    public static void ListenEvents(Events events, Action action)
    {
        if (Instance == null) return;
        Action thisEvent;
        if (Instance.m_eventDic.TryGetValue(events, out thisEvent))
        {
            thisEvent += action;

            Instance.m_eventDic[events] = thisEvent;
        }
        else
        {
            thisEvent += action;
            Instance.m_eventDic.Add(events, thisEvent);
        }
    }

    /// <summary>
    /// 登録したイベントを抹消する
    /// </summary>
    /// <param name="events"> イベントの種類 </param>
    /// <param name="action"> 抹消する処理 </param>
    public static void RemoveEvents(Events events, Action action)
    {
        if (Instance == null) return;
        Action thisEvent;
        if (Instance.m_eventDic.TryGetValue(events, out thisEvent))
        {
            thisEvent -= action;

            Instance.m_eventDic[events] = thisEvent;
        }
    }

    /// <summary>
    /// イベントを実行する
    /// </summary>
    /// <param name="events"> 実行するイベント </param>
    public static void OnEvent(Events events)
    {
        Action thisEvent;
        if (Instance.m_eventDic.TryGetValue(events, out thisEvent))
        {
            thisEvent?.Invoke();
        }
        else
        {
            return;
        }
    }
}
