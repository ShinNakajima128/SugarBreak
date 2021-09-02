using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimEvent : MonoBehaviour
{
    [SerializeField] EnemyData dragonData = default;
    [SerializeField] Dragon m_dragon = default;
    [SerializeField] BoxCollider m_boxCollider = default;
    [SerializeField] float m_knockbackPower = 2;

    private void Start()
    {
        m_boxCollider.enabled = false;
    }

    private void OnEnable()
    {
        m_dragon.AttackStartAction += AttackStart;
        m_dragon.AttackEndAction += AttackEnd;
        m_dragon.StateEndAction += StateEnd;
    }

    public void AttackStart()
    {
        m_boxCollider.enabled = true;
    }

    public void AttackEnd()
    {
        m_boxCollider.enabled = false;
    }

    public void StateEnd()
    {
        m_dragon.SetState(DragonState.Freeze);
    }

    public void EndDamage()
    {
        m_dragon.SetState(DragonState.Chase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var target = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<IDamagable>();
            var rb = other.gameObject.GetComponent<Rigidbody>();
            if (target != null)
            {
                rb.AddForce(rb.transform.forward * m_knockbackPower * -1, ForceMode.Impulse);
                target.Damage(dragonData.atk);
            }
        }
    }
}
