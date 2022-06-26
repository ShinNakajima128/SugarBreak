using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyWeapon : WeaponBase, IWeapon
{
    void IWeapon.WeaponAction1(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }

    void IWeapon.WeaponAction2(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }

    void IWeapon.WeaponAction3(Animator anim, Rigidbody rb)
    {
        throw new System.NotImplementedException();
    }
}
