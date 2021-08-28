﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    [SerializeField] float m_rotate_Y = -0.5f;
    [SerializeField] PlayerData playerData = default;
    [SerializeField] HpGauge hpGauge = default;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }
    void Update()
    {
        transform.Rotate(new Vector3(0f, m_rotate_Y, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerData.MaxHp += 2;
            playerData.HP = playerData.MaxHp;
            hpGauge.SetHpGauge(playerData.HP);
            soundManager.PlaySeByName("Heal");
            Destroy(this.gameObject);
        }
    }
}
