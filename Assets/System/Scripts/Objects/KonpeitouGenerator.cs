using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonpeitouGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] m_konpeito = null;
    [SerializeField] int m_generateNum = 10;
    [SerializeField] float m_generateTime = 0.1f;
    [SerializeField] float m_generatePower = 10;
    [SerializeField] Transform m_targetObject = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateKonpeitou(this.transform, m_generateNum);
        }
    }

    public void GenerateKonpeitou(Transform enemyTfm, int generateNum)
    {
        StartCoroutine(GenerateInterval(enemyTfm, generateNum));
    }

    IEnumerator GenerateInterval(Transform enemy, int geneNum)
    {
        for (int i = 0; i < geneNum; i++)
        {
            yield return new WaitForSeconds(m_generateTime);

            var kon = Instantiate(m_konpeito[Random.Range(0, m_konpeito.Length)], transform);
            kon.GetComponent<Konpeitou>().m_target = m_targetObject;
            kon.GetComponent<Konpeitou>().m_position = enemy.position;
            var m_rb = kon.gameObject.GetComponent<Rigidbody>();
            Vector3 force = new Vector3(Random.Range(-3, 3), m_generatePower, Random.Range(-3, 3));

            m_rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
