using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBox : ItemboxBase
{
    public override void Damage(int attackDamage)
    {
        base.Damage(attackDamage);
        Debug.Log("NormalBoxに当たった");
    }
}
