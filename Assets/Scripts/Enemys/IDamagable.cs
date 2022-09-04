using UnityEngine;
public interface IDamagable
{
    Transform EffectTarget { get; }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="attackPower"> 受けた攻撃力 </param>
    /// <param name="blowUpDir"> 吹き飛ばされる方向 </param
    /// /// <param name="blowUpPower"> 吹き飛ばす力</param>
    void Damage(int attackPower, Rigidbody hitRb = null, Vector3 blowUpDir = default, float blowUpPower = 1);
}