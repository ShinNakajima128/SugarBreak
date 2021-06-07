using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konpeitou : MonoBehaviour
{
    [SerializeField] LayerMask PlayerLayer;
    [SerializeField] float m_moveSpeed = 0.5f;
    Vector3 m_playerPosition;
    bool isTriggered = false;

    private void Update()
    {
        if (isTriggered)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, m_playerPosition, m_moveSpeed);

            if (transform.position == m_playerPosition)
            {
                GameManager.totalKonpeitou++;
                Destroy(this.gameObject);
            }
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_playerPosition = other.transform.position;
            isTriggered = true;
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    //Debug.Log(collision.gameObject.name);

    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}
