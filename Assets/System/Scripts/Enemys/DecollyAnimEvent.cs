using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecollyAnimEvent : MonoBehaviour
{
    [SerializeField] Decolly decolly = default;
    [SerializeField] BoxCollider m_boxCollider = default;

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
}
