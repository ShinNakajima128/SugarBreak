using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konpeitou : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
            Destroy(this.gameObject);
        }
    }
}
