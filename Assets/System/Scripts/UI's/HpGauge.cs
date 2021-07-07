using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGauge : MonoBehaviour
{
    [SerializeField] GameObject m_evenHpObj = default;
    [SerializeField] GameObject m_addHpObj = default;
    [SerializeField] PlayerData playerData = default;

    public void SetHpGauge(int hp)
    {
        //　体力を一旦全削除
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //　現在の体力数分のライフゲージを作成
        for (int i = 1; i <= hp; i++)
        {
            if (i == hp && i % 2 == 1)
            {
                Instantiate<GameObject>(m_addHpObj, transform);

            }
            else if (i % 2 == 0)
            {
                Instantiate<GameObject>(m_evenHpObj, transform);

            }
        }
    }

    //　ダメージ分だけ削除
    public void SetDamageHpGauge(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            //　最後のライフゲージを削除
            Destroy(transform.GetChild(i).gameObject);
            //Destroy(transform.GetChild(transform.childCount - 1 - i).gameObject);
        }
    }
}
