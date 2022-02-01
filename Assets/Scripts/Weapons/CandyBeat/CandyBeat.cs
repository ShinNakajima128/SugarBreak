using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器「キャンディビート」の機能を持つクラス
/// </summary>
public class CandyBeat : WeaponBase, IWeapon
{
    [SerializeField] float m_hitStopTime = 0.2f;
    Coroutine coroutine;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(attackDamage);

            if (coroutine == null)
            {
               coroutine = StartCoroutine(HitStop());
            }
        }
    }

    IEnumerator HitStop()
    {
        Time.timeScale = 0.01f;

        yield return new WaitForSecondsRealtime(m_hitStopTime);

        Time.timeScale = 1.0f;

        coroutine = null;
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="rb"></param>
    public void WeaponAction1(Animator anim, Rigidbody rb, int comboNum = 0)
    {
        anim.SetBool("Light", true);
    }

    /// <summary>
    /// ジャンプ強攻撃
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="rb"></param>
    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        anim.SetBool("Strong", true);
    }

    /// <summary>
    /// 考え中
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="rb"></param>
    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }
}
