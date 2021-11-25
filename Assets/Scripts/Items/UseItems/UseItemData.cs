using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "MyScriptable/Create UseItemData")]
public class UseItemData : UseItemBase
{
    public override void Use(int hp)
    {
        base.Use(hp);
    }
}
