using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「ポップランチャー」の弾の機能を持つクラス
/// </summary>
public class PopBullet : MonoBehaviour
{
    int m_attackDamage = 0;

    public int AttackDamage
    {
        get { return m_attackDamage; }
        set { m_attackDamage = value; }
    }

    private void OnCollisionEnter(Collision other)
    {
        EffectManager.PlayEffect(EffectType.Explosion, this.transform.position);
        AudioManager.PlaySE(SEType.Weapon_Explosion);
        EventManager.OnEvent(Events.CameraShake); //カメラを揺らす
        Destroy(this.gameObject);
    }
}
