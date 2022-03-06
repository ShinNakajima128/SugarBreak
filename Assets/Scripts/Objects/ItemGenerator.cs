using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムを生成する機能を持つクラス
/// </summary>
public class ItemGenerator : MonoBehaviour
{
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
    
    Transform m_targetObject = null;

    GameObject[] m_generateKon;
    public static ItemGenerator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_targetObject = GameObject.FindGameObjectWithTag("Target").GetComponent<Transform>();
        m_generateKon = new GameObject[m_generateNum];

        for (int i = 0; i < m_generateNum; i++)
        {
            var r = Random.Range(0, m_konpeitous.Length);

            m_generateKon[i] = Instantiate(m_konpeitous[r], this.transform);

            m_generateKon[i].SetActive(false);
        }
    }

    /// <summary>
    /// 金平糖を生成する
    /// </summary>
    /// <param name="generateNum"> 生成する数 </param>
    /// <param name="pos"> 生成する位置 </param>
    public void GenerateKonpeitou(int generateNum, Vector3 pos)
    {
        StartCoroutine(GenerateInterval(generateNum, pos, m_generatePower));
    }

    /// <summary>
    /// チョコエッグを生成する
    /// </summary>
    /// <param name="tfm"> 生成する位置 </param>
    public void GenerateChocoEgg(Transform tfm)
    {
        var egg = Instantiate(m_chocoEgg, tfm.position, tfm.rotation);
        egg.GetComponentInChildren<ChocoEgg>().BossTypes = BossType.Dragon;
    }

    IEnumerator GenerateInterval(int num, Vector3 pos, float power)
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
                    kon.GetComponent<Konpeitou>().Target = m_targetObject;
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
