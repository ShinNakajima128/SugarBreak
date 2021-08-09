using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGauge : MonoBehaviour
{
    /// <summary> 体力が偶数時の体力オブジェクト </summary>
    [SerializeField] GameObject m_evenHpObj = default;
    /// <summary> 体力が奇数時の体力オブジェクト </summary>
    [SerializeField] GameObject m_addHpObj = default;
    /// <summary> プレイヤーのデータ </summary>
    [SerializeField] PlayerData playerData = default;

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
}
