using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

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

    [SerializeField]
    float m_aimSpeed = 0.1f;

    Rigidbody m_playerRb = default;
    CinemachineVirtualCamera m_aimingCamera = default;
    CinemachineBrain m_brain = default;
    bool m_init = false;
    public Vector3 ShootVelocity => m_muzzle.transform.forward * m_shootPower;
    public Vector3 InstantiatePosition => m_muzzle.transform.position;

    private void OnEnable()
    {
        if (!m_init)
        {
            m_playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            m_aimingCamera = GameObject.FindGameObjectWithTag("AimingCamera").GetComponent<CinemachineVirtualCamera>();
            m_brain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
            m_init = true;
        }
        WeaponActionManager.ListenAction(ActionType.Action1, ShootBullet);
    }

    private void OnDisable()
    {
        WeaponActionManager.RemoveAction(ActionType.Action1, ShootBullet);
        AimRotation.Instance.ResetWeaponListRotation();
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
        if (PlayerController.Instance.IsAimed)
        {
            anim.SetBool("AimShoot", true);
        }
        else
        {
            anim.SetBool("Shoot", true);
        }
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        PlayerStatesManager.Instance.IsOperation = false;
        StartCoroutine(PlayerController.Instance.AttackMotionTimer(1.0f));
    }

    public void WeaponAction2(Animator anim, Rigidbody rb)
    {
        Debug.Log("ポップランチャージャンプ時攻撃");
    }

    public void WeaponAction3(Animator anim, Rigidbody rb)
    {
        StartCoroutine(BrendSpeedChange());

        if (!PlayerController.Instance.IsAimed)
        {
            m_aimingCamera.Priority = 30;
            anim.SetBool("isAimed", true);
        }
        else
        {
            m_aimingCamera.Priority = 10;
            anim.SetBool("isAimed", false);
            rb.gameObject.transform.rotation = Quaternion.Euler(0, rb.gameObject.transform.rotation.y, 0);
        }
        PlayerController.Instance.IsAimed = !PlayerController.Instance.IsAimed;
    }
    IEnumerator BrendSpeedChange()
    {
        m_brain.m_DefaultBlend.m_Time = m_aimSpeed;
        yield return new WaitForSeconds(m_aimSpeed);
        m_brain.m_DefaultBlend.m_Time = 1.0f;
    }
}
