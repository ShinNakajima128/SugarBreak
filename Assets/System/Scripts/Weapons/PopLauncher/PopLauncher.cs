using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopLauncher : WeaponBase
{
    [SerializeField] GameObject m_muzzle = null;
    [SerializeField] GameObject m_bulletPrefab = null;
    [SerializeField] float m_shootPower = 5.0f;
    AnimationEventScript animationEvent;

    private void Awake()
    {
        animationEvent = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimationEventScript>();   
    }

    private void OnEnable()
    {
        animationEvent.AttackAction += ShootBullet;
    }

    public  void ShootBullet()
    {
        var bullet = Instantiate(m_bulletPrefab, m_muzzle.transform.position, m_muzzle.transform.rotation);
        bullet.GetComponent<PopBullet>().AttackDamage = attackDamage;
        var m_rb = bullet.GetComponent<Rigidbody>();
        m_rb.AddForce(bullet.transform.forward * m_shootPower, ForceMode.Impulse);
    }
}
