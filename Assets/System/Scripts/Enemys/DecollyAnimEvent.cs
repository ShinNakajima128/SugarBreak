using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecollyAnimEvent : MonoBehaviour
{
    Decolly decolly;
    [SerializeField] BoxCollider m_boxCollider = default;

    void Start()
    {
        decolly = GetComponent<Decolly>();
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
