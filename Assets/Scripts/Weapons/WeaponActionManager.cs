using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 攻撃のアクションの種類
/// </summary>
public enum ActionType
{
    /// <summary> 攻撃1 </summary>
    Action1,
    /// <summary> 攻撃2 </summary>
    Action2,
    /// <summary> 攻撃3 </summary>
    Action3,
    /// <summary> 当たり判定開始 </summary>
    StartHitDecision,
    /// <summary> 当たり判定終了 </summary>
    FinishHitDecision,
    /// <summary> 各武器のエフェクトを再生 </summary>
    WeaponEffect
}

/// <summary>
/// アニメーションイベントに設定する武器のアクションを管理するクラス
/// </summary>
public class WeaponActionManager : MonoBehaviour
{
    /// <summary> 攻撃1の機能を持つUnityEvent </summary>
    UnityEvent m_weaponAction1 = new UnityEvent();
    /// <summary> 攻撃2の機能を持つUnityEvent </summary>
    UnityEvent m_weaponAction2 = new UnityEvent();
    /// <summary> 攻撃3の機能を持つUnityEvent </summary>
    UnityEvent m_weaponAction3 = new UnityEvent();
    /// <summary> 当たり判定開始の機能を持つUnityEvent </summary>
    UnityEvent m_startHitDecision = new UnityEvent();
    /// <summary> 当たり判定終了の機能を持つUnityEvent </summary>
    UnityEvent m_finishHitDecision = new UnityEvent();
    /// <summary> 各武器のエフェクト再生の機能を持つUnityEvent </summary>
    UnityEvent m_weaponEffect = new UnityEvent();

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
    /// 近接武器の当たり判定の受付を開始する
    /// </summary>
    public void OnStartHitDesition()
    {
        m_startHitDecision?.Invoke();
    }

    /// <summary>
    /// 近接武器の当たり判定の受付を終了する
    /// </summary>
    public void OnFinishHitDesition()
    {
        m_finishHitDecision?.Invoke();
    }

    public void OnWeaponEffect()
    {
        m_weaponEffect?.Invoke();
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
            case ActionType.StartHitDecision:
                instance.m_startHitDecision.AddListener(action);
                break;
            case ActionType.FinishHitDecision:
                instance.m_finishHitDecision.AddListener(action);
                break;
            case ActionType.WeaponEffect:
                instance.m_weaponEffect.AddListener(action);
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
            case ActionType.StartHitDecision:
                instance.m_startHitDecision.RemoveListener(action);
                break;
            case ActionType.FinishHitDecision:
                instance.m_finishHitDecision.RemoveListener(action);
                break;
            case ActionType.WeaponEffect:
                instance.m_weaponEffect.RemoveListener(action);
                break;
            default:
                Debug.LogError("値が不正なため、コードの記述ミスの可能性有");
                break;
        }
    }
}
