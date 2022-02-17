using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossMotionTest : MonoBehaviour
{
    [SerializeField]
    int m_attackPower = 2;
    
    [SerializeField]
    Transform m_walkEffectPos = default;

    [SerializeField]
    Transform m_attackEffectPos = default;

    [SerializeField]
    BoxCollider m_attackCollider = default;

    [SerializeField]
    HitDecision m_hd = default;

    public void Walk()
    {
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        SoundManager.Instance.PlaySeByName("怪獣の足音");
        EffectManager.PlayEffect(EffectType.Explosion, m_walkEffectPos.position);
    }

    public void Attack()
    {
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        SoundManager.Instance.PlaySeByName("全力で踏み込む");
        EffectManager.PlayEffect(EffectType.Landing, m_attackEffectPos.position);
        m_hd.AttackDamage = m_attackPower;
    }

    public void OnAttack1Collider()
    {
        m_attackCollider.enabled = true;
    }

    public void OffAttack1Collider()
    {
        m_attackCollider.enabled = false;
    }
}
