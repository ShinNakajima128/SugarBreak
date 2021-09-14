using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonpeitouGenerator : MonoBehaviour
{
    public static KonpeitouGenerator Instance { get; private set; }

    [Header("金平糖のプレハブ")]
    [SerializeField] 
    GameObject[] m_konpeitous = null;

    [Header("チョコエッグのプレハブ")]
    [SerializeField]
    GameObject m_chocoEgg = default;
    
    [Header("オブジェクトプールに入れる金平糖の数")]
    [SerializeField] 
    int m_generateNum = 100;
    
    [Header("生成時間")]
    [SerializeField] 
    float m_generateTime = 0.1f;
    
    [Header("生成する時に飛ぶ力")]
    [SerializeField] 
    float m_generatePower = 10;
    
    [SerializeField] 
    Transform m_targetObject = null;

    GameObject[] m_generateKon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_generateKon = new GameObject[m_generateNum];

        for (int i = 0; i < m_generateNum; i++)
        {
            var r = Random.Range(0, m_konpeitous.Length);

            m_generateKon[i] = Instantiate(m_konpeitous[r], this.transform);

            m_generateKon[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateKonpeitou(5, m_targetObject.position);
        }
    }

    public void GenerateKonpeitou(int generateNum, Vector3 pos)
    {
        StartCoroutine(GenerateInterval(generateNum, pos, m_generatePower));
    }

    public void GenerateChocoEgg(Transform tfm)
    {
        var egg = Instantiate(m_chocoEgg, tfm.position, m_chocoEgg.transform.rotation);
        egg.GetComponentInChildren<ChocoEgg>().BossTypes = BossType.Dragon;
    }

    IEnumerator GenerateInterval(int num, Vector3 pos, float power)
    {
        //Vector3 pos = startPos;

        //for (int i = 0; i < geneNum; i++)
        //{
        //    yield return new WaitForSeconds(m_generateTime);

        //    var kon = Instantiate(m_konpeitous[Random.Range(0, m_konpeitous.Length)], pos, Quaternion.identity, transform);
        //    var konpei = kon.GetComponent<Konpeitou>();
        //    konpei.m_target = m_targetObject;
        //    konpei.m_position = pos;
        //    var m_rb = kon.gameObject.GetComponent<Rigidbody>();
        //    Vector3 force = new Vector3(Random.Range(-2, 2), power, Random.Range(-2, 2));

        //    m_rb.AddForce(force, ForceMode.Impulse);
        //}
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
                    kon.GetComponent<Konpeitou>().m_target = m_targetObject;
                    var m_rb = kon.gameObject.GetComponent<Rigidbody>();
                    m_rb.velocity = Vector3.zero;
                    Vector3 force = new Vector3(Random.Range(-2, 2), power, Random.Range(-2, 2));
                    m_rb.AddForce(force, ForceMode.Impulse);

                    yield return new WaitForSeconds(m_generateTime);
                }
            }
        }

        if (Konpeitou.playSeCount >= 7)
        {
            Konpeitou.playSeCount = 0;
        }
    }
}
