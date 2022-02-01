using UnityEngine;

public interface IWeapon
{
    void WeaponAction1(Animator anim, Rigidbody rb, int comboNum = 0);
    void WeaponAction2(Animator anim, Rigidbody rb);
    void WeaponAction3(Animator anim, Rigidbody rb);
}
