using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Cinemachine;

public class TalkTrigger : MonoBehaviour
{
    [SerializeField] Flowchart m_flowchart = default;
    [SerializeField] string m_TalkChart = default;
    [SerializeField] CinemachineFreeLook freeLook = default;
    bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isActivated)
        {
            m_flowchart.SendFungusMessage(m_TalkChart);
            isActivated = true;
        }
    }

    public void ActiveCamera()
    {
        freeLook.Priority = 12;
    }

    public void InactiveCamera()
    {
        freeLook.Priority = 9;
    }

}
