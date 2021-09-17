using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGauge : MonoBehaviour
{
    [Header("1番目の偶数体力")]
    [SerializeField] 
    GameObject m_evenHpObj1 = default;

    [Header("2番目の偶数体力")]
    [SerializeField]
    GameObject m_evenHpObj2 = default;

    [Header("3番目の偶数体力")]
    [SerializeField]
    GameObject m_evenHpObj3 = default;

    [Header("1番目の奇数体力")]
    [SerializeField] 
    GameObject m_addHpObj1 = default;

    [Header("2番目の奇数体力")]
    [SerializeField]
    GameObject m_addHpObj2 = default;

    [Header("3番目の奇数体力")]
    [SerializeField]
    GameObject m_addHpObj3 = default;

    /// <summary> プレイヤーのデータ </summary>
    [SerializeField]
    PlayerData playerData = default;

    /// <summary>
    /// HPの数値に応じて体力オブジェクトをセットする
    /// </summary>
    /// <param name="hp"> HPの値 </param>
    public void SetHpGauge(int hp)
    {
        ///　体力を一旦全削除
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        ///　現在の体力数分のライフゲージを作成
        //for (int i = 1; i <= hp; i++)
        //{
        //    if (i == hp && i % 2 == 1)
        //    {
        //        Instantiate(m_addHpObj1, transform);

        //    }
        //    else if (i % 2 == 0)
        //    {
        //        Instantiate(m_evenHpObj1, transform);

        //    }
        //}

        for (int i = 1; i <= hp; i++)
        {
            if (i == hp && i % 2 == 1)
            {
                if (i == 7)
                {
                    Instantiate(m_addHpObj1, transform);
                }
                else if (i == 5)
                {
                    Instantiate(m_addHpObj3, transform);

                }
                else if (i == 3)
                {
                    Instantiate(m_addHpObj2, transform);

                }
                else if (i == 1)
                {
                    Instantiate(m_addHpObj1, transform);
                }


            }
            else if (i % 2 == 0)
            {
                if (i == 8)
                {
                    Instantiate(m_evenHpObj1, transform);
                }
                else if (i == 6)
                {
                    Instantiate(m_evenHpObj3, transform);

                }
                else if (i == 4)
                {
                    Instantiate(m_evenHpObj2, transform);

                }
                else if (i == 2)
                {
                    Instantiate(m_evenHpObj1, transform);
                }
            }
        }
    }
}
