using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopLauncher : WeaponBase
{
    [SerializeField] GameObject m_muzzle = null;
    [SerializeField] GameObject m_bullet = null;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(m_bullet, m_muzzle.transform);
            var bullet = 
        }
    }
}
