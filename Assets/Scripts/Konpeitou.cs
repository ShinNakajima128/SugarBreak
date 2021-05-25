using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konpeitou : MonoBehaviour
{
    [SerializeField] LayerMask PlayerLayer;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.layer == PlayerLayer)
        {
            Destroy(this.gameObject);
        }
    }
}
