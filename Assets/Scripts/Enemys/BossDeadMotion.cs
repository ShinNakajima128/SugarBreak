using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadMotion : MonoBehaviour
{
    Rigidbody m_rb = default;
    MeshCollider[] m_colliders = default;
    MeshRenderer[] m_meshs = default;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_colliders = GetComponentsInChildren<MeshCollider>();
        m_meshs = GetComponentsInChildren<MeshRenderer>();
    }

    IEnumerator StartDeadMmotion()
    {
        EffectManager.PlayEffect(EffectType.BossDead, transform.position);
        foreach (var m in m_meshs)
        {
            StartCoroutine(Disappear(m));
            yield return null;
        }
    }

    IEnumerator Disappear(MeshRenderer mesh)
    {
        float a = 1.0f;
        while (mesh.material.color.a > 0)
        {
            Color c = new Color(0, 0, 0, a);
            mesh.material.color = c;
            a -= 0.01f * Time.deltaTime;
            yield return null;
        }
    }
}
