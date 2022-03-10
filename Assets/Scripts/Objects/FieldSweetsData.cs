using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドに存在するお菓子の種類
/// </summary>
public enum FieldSweetsType
{
    None,
    Shortcake,
    Waffle,
    Biscuit,
    Marshmallow
}

[CreateAssetMenu(menuName = "MyScriptable/Create FieldObjectData")]
public class FieldSweetsData : ScriptableObject
{
    [SerializeField]
    FieldSweetsType m_sweetsType = default;

    [SerializeField]
    Vector3 m_colliderSize = default;

    [SerializeField]
    int m_attackPower = 0;

    public FieldSweetsType SweetsType => m_sweetsType;

    public Vector3 ColliderSize => m_colliderSize;
    public int AttackPower => m_attackPower;
}
