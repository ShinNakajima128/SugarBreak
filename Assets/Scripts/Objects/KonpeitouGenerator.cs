using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonpeitouGenerator : MonoBehaviour
{
    public static KonpeitouGenerator Instance { get; private set; }
    [SerializeField] GameObject[] m_konpeito = null;
    [SerializeField] int m_generateNum = 10;
    [SerializeField] float m_generateTime = 0.1f;
    [SerializeField] float m_generatePower = 10;
    [SerializeField] Transform m_targetObject = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateKonpeitou(this.transform, m_generateNum, m_generatePower);
        }
    }

    public void GenerateKonpeitou(Transform Tfm, int generateNum, float power)
    {
        StartCoroutine(GenerateInterval(Tfm, generateNum, power));
    }

    IEnumerator GenerateInterval(Transform enemy, int geneNum, float power)
    {
        for (int i = 0; i < geneNum; i++)
        {
            yield return new WaitForSeconds(m_generateTime);

            var kon = Instantiate(m_konpeito[Random.Range(0, m_konpeito.Length)], enemy.position, Quaternion.identity, transform);
            var konpei = kon.GetComponent<Konpeitou>();
            konpei.m_target = m_targetObject;
            konpei.m_position = enemy.position;
            var m_rb = kon.gameObject.GetComponent<Rigidbody>();
            Vector3 force = new Vector3(Random.Range(-2, 2), power, Random.Range(-2, 2));

            m_rb.AddForce(force, ForceMode.Impulse);
        }

        if (Konpeitou.playSeCount >= 7)
        {
            Konpeitou.playSeCount = 0;
        }
    }
}
