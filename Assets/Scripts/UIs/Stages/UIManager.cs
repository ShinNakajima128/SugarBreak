using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] 
    TextMeshProUGUI m_stageName = default;

    [SerializeField]
    GameObject m_bossUI = default;

    public GameObject BossUI { get => m_bossUI; set => m_bossUI = value; }

    private void Awake()
    {
        Instance = this;
    }

    public void EnableStageName()
    {
        m_stageName.enabled = true;
        m_stageName.GetComponent<TmpAnimation>().StartPlay();
    }
    public void DisableStageName()
    {
        m_stageName.enabled = false;
    }
}
