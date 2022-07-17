using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMat : MonoBehaviour
{
    [SerializeField] float m_jumpPower = 10.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Rigidbody>();
            collision.gameObject.GetComponent<PlayerController>().JumpMotion();
            player.AddForce(player.transform.up * m_jumpPower, ForceMode.Impulse);
            AudioManager.PlaySE(SEType.FieldObject_JumpMat);
        }
    }
}
