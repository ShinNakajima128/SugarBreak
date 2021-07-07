using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatesManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData = default;
    [SerializeField] Text m_totalKonpeitou = default;
    [SerializeField] Text m_hpText = default;
    public static bool isOperation = true;

    void Update()
    {
        m_hpText.text = playerData.HP.ToString();
        m_totalKonpeitou.text = "× " + playerData.TotalKonpeitou.ToString();

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerData.HP++;
        }
    }
}
