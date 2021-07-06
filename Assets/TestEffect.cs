using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Test();
        }
    }

    public void Test()
    {
        EffectManager.PlayEffect(EffectType.BossDead, this.transform.position);
    }
}
