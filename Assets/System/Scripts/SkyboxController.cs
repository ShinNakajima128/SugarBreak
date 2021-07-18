using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [SerializeField] float anglePerFrame = 0.1f;
    float rot = 0.0f;
    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            rot += anglePerFrame;
            if (rot >= 360.0f)
            {
                rot -= 360.0f;
            }
            RenderSettings.skybox.SetFloat("_Rotation", rot);
            timer = 0;
        }
    }
}
