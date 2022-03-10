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

[CreateAssetMenu(menuName = "MyScriptable/Create FieldObjectData")]
public class FieldSweetsData : ScriptableObject
{
    [SerializeField]
    FieldSweetsSize m_sweetsSize = default;

    [SerializeField]
    int m_attackPower = 0;

    public FieldSweetsSize SweetsSize => m_sweetsSize;

    public Vector3 ColliderSize => GetSweetsSize(m_sweetsSize);
    public int AttackPower => m_attackPower;

    Vector3 GetSweetsSize(FieldSweetsSize sweetSize)
    {
        Vector3 size = default;
        switch (sweetSize)
        {
            case FieldSweetsSize.None:
                size = default;
                break;
            case FieldSweetsSize.Small:
                size = new Vector3(2, 2, 2);
                break;
            case FieldSweetsSize.Medium:
                size = new Vector3(2.5f, 2.5f, 2.5f);
                break;
            case FieldSweetsSize.Large:
                size = new Vector3(3, 3, 3);
                break;
            case FieldSweetsSize.ExtraLarge:
                size = new Vector3(3.5f, 3.5f, 3.5f);
                break;
        }
        return size;
    }
}
