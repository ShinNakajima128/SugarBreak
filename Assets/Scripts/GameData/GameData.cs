using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField]
    PlayerData m_playerData = default;

    [SerializeField]
    Stage[] m_Stage = default;
}
