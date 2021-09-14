using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonpeitouPool : MonoBehaviour
{
    [SerializeField]
    int m_generateNum = 100;

    [SerializeField]
    GameObject[] m_konpeutous = default;

    [SerializeField]
    Transform m_playerPos = default;

    [SerializeField]
    Transform m_enemyPos = default;

    GameObject[] m_generateKon;
    void Start()
    {
        m_generateKon = new GameObject[m_generateNum];

        for (int i = 0; i < m_generateNum; i++)
        {
            var r = Random.Range(0, m_konpeutous.Length);

            m_generateKon[i] = Instantiate(m_konpeutous[r], this.transform);

            m_generateKon[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateKonpeitou(5);
        }
    }

    void GenerateKonpeitou(int num)
    {
        StartCoroutine(Generate(num, m_enemyPos.position));
    }

    IEnumerator Generate(int num, Vector3 pos)
    {
        foreach (var kon in m_generateKon)
        {
            if (num <= 0)
            {
                break;
            }
            else
            {
                if (kon.activeSelf)
                {
                    continue;
                }
                else
                {
                    num--;
                    kon.SetActive(true);
                    kon.transform.position = pos;
                    kon.GetComponent<Konpeitou>().m_target = m_playerPos;
                    yield return new WaitForSeconds(0.05f);
                }
            }     
        }
    }
}
