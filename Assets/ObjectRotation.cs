using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] float m_rotateSpeed = default;
    
    void Update()
    {
        transform.Rotate(new Vector3(0, m_rotateSpeed, 0));
    }
}
