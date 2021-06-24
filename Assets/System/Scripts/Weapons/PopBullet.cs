using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBullet : MonoBehaviour
{
    [SerializeField] GameObject m_ExplosionSfx = null;
    Rigidbody m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(m_ExplosionSfx, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
