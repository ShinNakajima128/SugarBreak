using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスの機能のテスト用クラス
/// </summary>
public class BossMotionTest : MonoBehaviour
{
    /// <summary> 攻撃力 </summary>
    [SerializeField]
    int m_attackPower = 2;

    /// <summary> 移動速度 </summary>
    [SerializeField]
    float m_moveSpeed = 5.0f;

    /// <summary> 歩行時のエフェクトを出す位置 </summary>
    [SerializeField]
    Transform m_walkEffectPos = default;

    /// <summary> 攻撃のエフェクトを出す位置 </summary>
    [SerializeField]
    Transform m_attackEffectPos = default;

    /// <summary> 攻撃の当たり判定用のCollider </summary>
    [SerializeField]
    BoxCollider m_attackCollider = default;

    /// <summary> Playerにヒットしたか判定するクラス </summary>
    [SerializeField]
    HitDecision m_hd = default;

    CharacterController m_cc = default;
    PlayerSearcher m_ps = default;

    void Start()
    {
        
    }

    /// <summary>
    /// 歩く
    /// </summary>
    public void Walk()
    {
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        SoundManager.Instance.PlaySeByName("怪獣の足音");
        EffectManager.PlayEffect(EffectType.Landing, m_walkEffectPos.position);
    }

    /// <summary>
    /// 攻撃1
    /// </summary>
    public void Attack1()
    {
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        SoundManager.Instance.PlaySeByName("全力で踏み込む");
        EffectManager.PlayEffect(EffectType.Landing, m_attackEffectPos.position);
        m_hd.AttackDamage = m_attackPower;
    }

    /// <summary>
    /// 攻撃1の当たり判定をONにする
    /// </summary>
    public void OnAttack1Collider()
    {
        m_attackCollider.enabled = true;
    }
    /// <summary>
    /// 攻撃1の当たり判定をOFFにする
    /// </summary>
    public void OffAttack1Collider()
    {
        m_attackCollider.enabled = false;
    }
}
