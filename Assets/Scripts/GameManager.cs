using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int totalKonpeitou = 0;
    [SerializeField] Text m_konpeitous = null;


    void Update()
    {
        if (m_konpeitous != null) m_konpeitous.text = totalKonpeitou.ToString() + "個";
    }
}
