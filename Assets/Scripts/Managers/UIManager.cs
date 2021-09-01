using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject m_stageName = default;

    public void EnableStageName()
    {
        m_stageName.SetActive(true);
    }
    public void DisableStageName()
    {
        m_stageName.SetActive(false);
    }
}
