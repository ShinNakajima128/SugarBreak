using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonpeitouGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] m_konpeito = null;
    [SerializeField] int GenerateNum = 10;

    void Start()
    {
        for (int i = 0; i < GenerateNum; i++)
        {
            var kon = Instantiate(m_konpeito[Random.Range(0, m_konpeito.Length)], this.gameObject.transform);
            var m_rb = kon.gameObject.GetComponent<Rigidbody>();
            Vector3 force = new Vector3(0,Random.Range(5, 10), 0);
            
            m_rb.AddForce(force, ForceMode.Impulse);
        }
    }

    IEnumerator InstantiateTimer()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
