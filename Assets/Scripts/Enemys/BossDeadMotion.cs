using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossDeadMotion : MonoBehaviour
{
    [SerializeField]
    float m_disappearTime = 0.2f;

    [SerializeField]
    float m_explosionTime = 2.0f;

    Rigidbody[] m_rbs = default;
    MeshRenderer[] m_meshs = default;

    void Start()
    {
        m_rbs = GetComponentsInChildren<Rigidbody>();
        m_meshs = GetComponentsInChildren<MeshRenderer>();
        StartCoroutine(StartDeadMotion());
        for (int i = 0; i < m_rbs.Length; i++)
        {
            m_rbs[i].Sleep();
        }
    }

    IEnumerator StartDeadMotion()
    {
        SoundManager.Instance.PlaySeByName("BossDown");
        yield return new WaitForSeconds(m_explosionTime);

        for (int i = 0; i < m_rbs.Length; i++)
        {
            m_rbs[i].WakeUp();
        }
        EffectManager.PlayEffect(EffectType.BossDead, transform.position);
        ItemGenerator.Instance.GenerateChocoEgg(transform);
        EventManager.OnEvent(Events.BossBattleEnd);
        SoundManager.Instance.PlaySeByName("BossDead");

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
            var c = new Color(1, 1, 1, a);
            mesh.material.color = c;
            a -= m_disappearTime * Time.deltaTime;
            yield return null;
        }
        Destroy(mesh.gameObject);
    }
}
