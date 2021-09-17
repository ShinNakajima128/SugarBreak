using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] 
    GameObject m_stageName = default;

    [SerializeField]
    GameObject m_bossUI = default;

    public GameObject BossUI { get => m_bossUI; set => m_bossUI = value; }

    private void Awake()
    {
        Instance = this;
    }

    public void EnableStageName()
    {
        m_stageName.SetActive(true);
    }
    public void DisableStageName()
    {
        m_stageName.SetActive(false);
    }
}
