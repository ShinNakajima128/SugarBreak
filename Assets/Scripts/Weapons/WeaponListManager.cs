using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponListManager : MonoBehaviour
{
    public static WeaponListManager Instance;

    List<Image> m_weaponIconList = new List<Image>();

    private void Awake()
    {
        Instance = this;

        var weaponList = GetComponentsInChildren<Image>();

        foreach (var image in weaponList)
        {
            Debug.Log(image.sprite.name);
            m_weaponIconList.Add(image);
        }
    }

    public void IconChange()
    {
        for (int i = 0; i < m_weaponIconList.Count; i++)
        {
            if (GameManager.Instance.CurrentWeaponList[i].IsActived)
            {
                m_weaponIconList[i].sprite = GameManager.Instance.CurrentWeaponList[i].WeaponData.ActiveWeaponImage;
            }
            else
            {
                m_weaponIconList[i].sprite = GameManager.Instance.CurrentWeaponList[i].WeaponData.DeactiveWeaponImage;
            }

        }
    }
}
