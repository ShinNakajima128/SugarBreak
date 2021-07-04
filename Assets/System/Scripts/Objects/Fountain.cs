using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    [SerializeField] AudioClip m_fountainSfx = default;
    SoundManager soundManager;

    private void Start()
    {
        //soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(m_fountainSfx, this.transform.position);
    }
}
