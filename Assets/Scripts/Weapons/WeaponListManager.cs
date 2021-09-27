using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponListManager : MonoBehaviour
{
    public static WeaponListManager Instance { get; private set; }

    [SerializeField]
    List<WeaponBase> m_currentWeaponList = new List<WeaponBase>();

    List<Image> m_weaponIconList = new List<Image>();

    private void Awake()
    {
        Instance = this;

        var weaponList = GetComponentsInChildren<Image>();

        foreach (var image in weaponList)
        {
            m_weaponIconList.Add(image);
        }
    }

    public void IconChange()
    {
        for (int i = 0; i < m_weaponIconList.Count; i++)
        {
            if (m_currentWeaponList[i].IsActived)
            {
                m_weaponIconList[i].sprite = m_currentWeaponList[i].WeaponData.ActiveWeaponImage;
            }
            else
            {
                m_weaponIconList[i].sprite = m_currentWeaponList[i].WeaponData.DeactiveWeaponImage;
            }
        }
    }
}
