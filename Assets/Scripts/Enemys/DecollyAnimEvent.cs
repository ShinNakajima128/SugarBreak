using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecollyAnimEvent : MonoBehaviour
{
    [SerializeField] EnemyData decollyData = default;
    [SerializeField] Decolly decolly = default;
    [SerializeField] BoxCollider m_boxCollider = default;
    [SerializeField] float m_knockbackPower = 2;

    private void Start()
    {
        m_boxCollider.enabled = false;
    }

    private void OnEnable()
    {
        decolly.AttackStartAction += AttackStart;
        decolly.AttackEndAction += AttackEnd;
        decolly.StateEndAction += StateEnd;
    }
    
    public void AttackStart()
    {
        AudioManager.PlaySE(SEType.Decolly_Attack);
        m_boxCollider.enabled = true;
    }

    public void AttackEnd()
    {
        m_boxCollider.enabled = false;
    }

    public void StateEnd()
    {
        decolly.SetState(DecollyState.Freeze);
    }

    public void EndDamage()
    {
        decolly.SetState(DecollyState.Move);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerController.Instance.IsDodged) return;

        if (other.gameObject.CompareTag("Player"))
        {
            var target = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<IDamagable>();
            var rb = other.gameObject.GetComponent<Rigidbody>();
            if (target != null)
            {
                target.Damage(decollyData.atk, rb, transform.forward, 1);
            }
        }
    }
}
