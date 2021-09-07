using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoEgg : MonoBehaviour, IDamagable
{
    [SerializeField]
    GameObject[] m_dropItems = default;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int attackPower)
    {
        throw new System.NotImplementedException();
    }
}
