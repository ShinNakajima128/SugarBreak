using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    [SerializeField] float m_rotate_Y = -0.5f;
    [SerializeField] PlayerData playerData = default;
    [SerializeField] HpGauge hpGauge = default;

    void Update()
    {
        transform.Rotate(new Vector3(0f, m_rotate_Y, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerData.HP = playerData.MaxHp;
            hpGauge.SetHpGauge(playerData.HP);
            AudioManager.PlaySE(SEType.Player_Heal);
            EffectManager.PlayEffect(EffectType.Heal, other.transform);
            Destroy(this.gameObject);
        }
    }
}

