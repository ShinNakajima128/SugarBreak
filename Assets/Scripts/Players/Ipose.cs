using UnityEngine;

public interface IPose
{
    void Pose(Animator anim, Rigidbody rigidbody);
    void WakeUp(Animator anim, Rigidbody rigidbody);
}
