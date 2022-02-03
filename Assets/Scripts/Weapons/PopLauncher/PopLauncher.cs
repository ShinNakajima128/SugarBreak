using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 武器「ポップランチャー」の機能を持つクラス
/// </summary>
public class PopLauncher : WeaponBase, IWeapon
{
    [SerializeField] 
    GameObject m_muzzle = null;

    [SerializeField] 
    GameObject m_bulletPrefab = null;

    [SerializeField] 
    float m_shootPower = 5.0f;

    [SerializeField] 
    float m_recoilPower = 5.0f;

    Rigidbody m_playerRb = default;

    private void OnEnable()
    {
        if (m_playerRb == null)
        {
            m_playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        }
        WeaponActionManager.ListenAction(ActionType.Action1, ShootBullet);
    }

    private void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.Action1, ShootBullet);
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    public  void ShootBullet()
    {
        var bullet = Instantiate(m_bulletPrefab, m_muzzle.transform.position, m_muzzle.transform.rotation);
        bullet.GetComponent<PopBullet>().AttackDamage = attackDamage;
        var m_rb = bullet.GetComponent<Rigidbody>();
        m_rb.AddForce(bullet.transform.forward * m_shootPower, ForceMode.Impulse);
        m_playerRb.AddForce(-m_playerRb.transform.forward * m_recoilPower, ForceMode.Impulse);
        SoundManager.Instance.PlaySeByName("Shoot");
    }

    /// <summary>
    /// 地上時の通常攻撃
    /// </summary>
    /// <param name="anim"> プレイヤーのAnimator </param>
    /// <param name="rb"> プレイヤーのRigidbody </param>
    public void WeaponAction1(Animator anim, Rigidbody rb)
    {
        anim.SetBool("Shoot", true);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        PlayerStatesManager.Instance.IsOperation = false;
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        throw new NotImplementedException();
    }

    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        throw new NotImplementedException();
    }
}
