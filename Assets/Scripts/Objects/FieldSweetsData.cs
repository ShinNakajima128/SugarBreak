using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドに存在するお菓子の種類
/// </summary>
public enum FieldSweetsSize
{
    /// <summary> 未設定 </summary>
    None,
    /// <summary> 小 </summary>
    Small,
    /// <summary> 中 </summary>
    Medium,
    /// <summary> 大 </summary>
    Large,
    /// <summary> 特大 </summary>
    ExtraLarge
}

/// <summary>
/// ステージにある刺すことができるお菓子のデータ
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/Create FieldObjectData")]
public class FieldSweetsData : ScriptableObject
{
    /// <summary> お菓子のサイズ </summary>
    [SerializeField]
    FieldSweetsSize m_sweetsSize = default;

    /// <summary> お菓子を装着した時のメイン武器の攻撃力 </summary>
    [SerializeField]
    int m_attackPower = 0;

    /// <summary> 破棄した時に獲得できる金平糖の数 </summary>
    [SerializeField]
    int m_konpeitoNum = 0;

    /// <summary> お菓子の耐久力 </summary>
    [SerializeField]
    int m_enduranceCount = 0;

    public FieldSweetsSize SweetsSize => m_sweetsSize;

    public Vector3 ColliderSize => GetSweetsSize(m_sweetsSize);
    public int AttackPower => m_attackPower;

    /// <summary>
    /// お菓子にサイズ毎のコライダーの大きさを取得する
    /// </summary>
    /// <param name="sweetSize"> お菓子のサイズ </param>
    /// <returns></returns>
    Vector3 GetSweetsSize(FieldSweetsSize sweetSize)
    {
        Vector3 size = default;
        switch (sweetSize)
        {
            case FieldSweetsSize.None:
                size = default;
                break;
            case FieldSweetsSize.Small:
                size = new Vector3(2.5f, 2.5f, 2.5f);
                break;
            case FieldSweetsSize.Medium:
                size = new Vector3(3f, 3f, 3f);
                break;
            case FieldSweetsSize.Large:
                size = new Vector3(3.5f, 3.5f, 3.5f);
                break;
            case FieldSweetsSize.ExtraLarge:
                size = new Vector3(4f, 4f, 4f);
                break;
        }
        return size;
    }
    public int KonpeitouNum => m_konpeitoNum;
    public int EnduranceCount => m_enduranceCount;
}
