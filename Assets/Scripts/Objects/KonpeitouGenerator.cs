using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonpeitouGenerator : MonoBehaviour
{
    public static KonpeitouGenerator Instance { get; private set; }

    [Header("金平糖のプレハブ")]
    [SerializeField] 
    GameObject[] m_konpeito = null;

    [Header("チョコエッグのプレハブ")]
    [SerializeField]
    GameObject m_chocoEgg = default;
    
    [Header("生成する金平糖の数")]
    [SerializeField] 
    int m_generateNum = 10;
    
    [Header("生成時間")]
    [SerializeField] 
    float m_generateTime = 0.1f;
    
    [Header("生成する時に飛ぶ力")]
    [SerializeField] 
    float m_generatePower = 10;
    
    [SerializeField] 
    Transform m_targetObject = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GenerateKonpeitou(this.transform, m_generateNum);
        }
    }

    public void GenerateKonpeitou(Transform Tfm, int generateNum)
    {
        StartCoroutine(GenerateInterval(Tfm.position, generateNum, m_generatePower));
    }

    public void GenerateChocoEgg(Transform tfm)
    {
        var egg = Instantiate(m_chocoEgg, tfm.position, m_chocoEgg.transform.rotation);
        egg.GetComponentInChildren<ChocoEgg>().BossTypes = BossType.Dragon;
    }

    IEnumerator GenerateInterval(Vector3 startPos, int geneNum, float power)
    {
        Vector3 pos = startPos;

        for (int i = 0; i < geneNum; i++)
        {
            yield return new WaitForSeconds(m_generateTime);

            var kon = Instantiate(m_konpeito[Random.Range(0, m_konpeito.Length)], pos, Quaternion.identity, transform);
            var konpei = kon.GetComponent<Konpeitou>();
            konpei.m_target = m_targetObject;
            konpei.m_position = pos;
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
