using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour
{
    /// <summary> 自身のParticleの入れ物 </summary>
    ParticleSystem[] m_particles = default;
    
    private void Awake()
    {    
        m_particles = GetComponentsInChildren<ParticleSystem>();
        gameObject.SetActive(false);
    }
    void Update()
    {
        foreach (var particle in m_particles)
        {
            if (particle.isPlaying)
            {
                return;
            }
        }
        gameObject.SetActive(false);//全てのParticleの再生終了で非アクティブにする
    }
    /// <summary>
    /// 指定した場所でEffectを再生する
    /// </summary>
    /// <param name="pos"></param>
    public void Play(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.localPosition = pos;
        foreach (var particle in m_particles)
        {
            particle.Play();
        }
    }
    /// <summary>
    /// Effectの再生を止める
    /// </summary>
    public void Stop()
    {
        foreach (var particle in m_particles)
        {
            particle.Stop();
        }
    }
    /// <summary>
    /// 再生中はTrueを返す
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
}
