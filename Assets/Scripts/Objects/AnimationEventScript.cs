using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventScript : MonoBehaviour
{
    [SerializeField] GameObject m_candyBeat = null;
    [SerializeField] AudioClip[] m_weaponSfxs = null;
    SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void CandyAttack()
    {
        m_candyBeat.GetComponent<BoxCollider>().enabled = true;
        soundManager.PlaySE(m_weaponSfxs[0]);
    }

     public void FinishCandyAttack()
    {
        m_candyBeat.GetComponent<BoxCollider>().enabled = false;
    }
}
