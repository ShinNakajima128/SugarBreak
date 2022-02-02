using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 攻撃のアクションの種類
/// </summary>
public enum ActionType
{
    Action1,
    Action2,
    Action3
}

/// <summary>
/// アニメーションイベントに設定する武器のアクションを管理するクラス
/// </summary>
public class WeaponActionManager : MonoBehaviour
{
    UnityEvent m_weaponAction1 = new UnityEvent();

    UnityEvent m_weaponAction2 = new UnityEvent();

    UnityEvent m_weaponAction3 = new UnityEvent();

    static WeaponActionManager instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 武器アクション1を実行する
    /// </summary>
    public void OnAction1()
    {
        m_weaponAction1?.Invoke();
    }

    /// <summary>
    /// 武器アクション2を実行する
    /// </summary>
    public void OnAction2()
    {
        m_weaponAction2?.Invoke();
    }

    /// <summary>
    /// 武器アクション3を実行する
    /// </summary>
    public void OnAction3()
    {
        m_weaponAction3?.Invoke();
    }

    /// <summary>
    /// 武器の機能を登録する
    /// </summary>
    /// <param name="type"> アクションの種類 </param>
    /// <param name="action"> 武器の機能 </param>
    public static void ListenAction(ActionType type, UnityAction action)
    {
        switch (type)
        {
            case ActionType.Action1:
                instance.m_weaponAction1.AddListener(action);
                break;
            case ActionType.Action2:
                instance.m_weaponAction2.AddListener(action);
                break;
            case ActionType.Action3:
                instance.m_weaponAction3.AddListener(action);
                break;
            default:
                Debug.LogError("値が不正なため、コードの記述ミスの可能性有");
                break;
        }
    }

    /// <summary>
    /// 武器の機能を消去する
    /// </summary>
    /// <param name="type"> アクションの種類 </param>
    /// <param name="action"> 消去する機能 </param>
    public static void RemoveAction(ActionType type, UnityAction action)
    {
        switch (type)
        {
            case ActionType.Action1:
                instance.m_weaponAction1.RemoveListener(action);
                break;
            case ActionType.Action2:
                instance.m_weaponAction2.RemoveListener(action);
                break;
            case ActionType.Action3:
                instance.m_weaponAction3.RemoveListener(action);
                break;
            default:
                Debug.LogError("値が不正なため、コードの記述ミスの可能性有");
                break;
        }
    }
}
