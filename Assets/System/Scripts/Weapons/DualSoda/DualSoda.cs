using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.System.Scripts.Damage;

public class DualSoda : WeaponBase
{
    [SerializeField] GameObject m_leftSoda = default;
    [SerializeField] GameObject m_rightSoda = default;
    [SerializeField] GameObject m_lefthand = default;
    [SerializeField] GameObject m_righthand = default;

    private void OnEnable()
    {
        m_leftSoda.SetActive(true);
        m_rightSoda.SetActive(true);
        //m_rightSoda.transform.Rotate(-30f, 9.7f, 149);
        m_leftSoda.transform.parent = m_lefthand.transform;
        m_rightSoda.transform.parent = m_righthand.transform;
    }
    private void OnDisable()
    {
        if (m_leftSoda.activeSelf) m_leftSoda.SetActive(false);
        if (m_rightSoda.activeSelf) m_rightSoda.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Damage(attackDamage);
        }
    }
}
