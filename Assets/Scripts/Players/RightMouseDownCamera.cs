using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RightMouseDownCamera : MonoBehaviour
{
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Camera X")
        {
            if (Input.GetMouseButton(1))
            {
                return Input.GetAxis("Camera X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Camera Y")
        {
            if (Input.GetMouseButton(1))
            {
                return Input.GetAxis("Camera Y");
            }
            else
            {
                return 0;
            }
        }
        return Input.GetAxis(axisName);
    }
}