using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour
{
    /// <summary> 自身のParticleの入れ物 </summary>
    ParticleSystem[] m_particles = default;

    bool m_isSetParent = false;
    Transform _effectMng;
    
    private void Awake()
    {    
        m_particles = GetComponentsInChildren<ParticleSystem>();
        gameObject.SetActive(false);
    }
    void Start()
    {
        _effectMng = GameObject.FindGameObjectWithTag("EffectManager").transform;
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
        if (m_isSetParent)
        {
            gameObject.transform.SetParent(_effectMng);
            gameObject.transform.localPosition = Vector3.zero;
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
    /// 指定した場所でEffectを再生する
    /// </summary>
    /// <param name="pos"></param>
    public void Play(Transform parent)
    {
        gameObject.SetActive(true);
        transform.SetParent(parent);
        m_isSetParent = true;
        transform.localPosition = Vector3.zero;
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
