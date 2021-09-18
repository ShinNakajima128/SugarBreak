using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消費アイテムの基底クラス
/// </summary>
public  abstract class UseItemBase : MonoBehaviour
{
    [SerializeField]
    protected UseItemData m_itemData = default;

    public abstract void UseItem();
}
