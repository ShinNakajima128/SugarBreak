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
            playerData.HP += 2;
            hpGauge.SetHpGauge(playerData.HP);
            Destroy(this.gameObject);
        }
    }
}

