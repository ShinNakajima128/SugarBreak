using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KonpeitouUI : MonoBehaviour
{
    [SerializeField] Text m_totalKonpeitou = null;

    void Update()
    {
        m_totalKonpeitou.text = "×" + Konpeitou.totalKonpeitou.ToString();
    }
}
