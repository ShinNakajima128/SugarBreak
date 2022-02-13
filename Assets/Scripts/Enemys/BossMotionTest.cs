using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossMotionTest : MonoBehaviour
{
    [SerializeField]
    Transform m_walkEffectPos = default;

    [SerializeField]
    Transform m_attackEffectPos = default;

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
    }
}
