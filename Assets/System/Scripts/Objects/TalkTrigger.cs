using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class TalkTrigger : MonoBehaviour
{
    [SerializeField] Flowchart m_flowchart = default;
    [SerializeField] string m_TalkChart = default;
    bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isActivated)
        {
            m_flowchart.SendFungusMessage(m_TalkChart);
            isActivated = true;
        }
    }

}
