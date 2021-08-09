using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMat : MonoBehaviour
{
    [SerializeField] float m_jumpPower = 10.0f;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Rigidbody>();
            player.AddForce(player.transform.up * m_jumpPower, ForceMode.Impulse);
            soundManager.PlaySeByName("JumpMat");
        }
    }
}
