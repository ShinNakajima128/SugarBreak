using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    [SerializeField]
    Transform m_effectPos = default;

    public void OnDashEffect()
    {
        EffectManager.PlayEffect(EffectType.Mokumoku, m_effectPos.position);
    }
}
