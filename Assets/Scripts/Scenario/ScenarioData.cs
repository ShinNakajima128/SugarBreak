using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create ScenarioData")]
public class ScenarioData : ScriptableObject
{
    [SerializeField]
    DialogData[] m_dialogData = default;

    public DialogData[] DialogData => m_dialogData; 
}

[Serializable]
public class DialogData
{
    public int Id;
    public string[] Messages;
}
