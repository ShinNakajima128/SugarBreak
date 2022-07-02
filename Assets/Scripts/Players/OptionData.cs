using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create OptionData")]
public class OptionData : ScriptableObject
{
    [SerializeField]
    SoundOption soundOption;

    public SoundOption SoundOptionData => soundOption;
}

[Serializable]
public class SoundOption
{
    public float MasterVolume;
    public float BgmVolume;
    public float SeVolume;
    public float VoiceVolume;
    public bool IsMasterMute;
    public bool IsBgmMute;
    public bool IsSeMute;
    public bool IsVoiceMute;

    public void SetData(GameData data)
    {
        MasterVolume = data.OptionData.Volumes.MasterVolume;
        BgmVolume = data.OptionData.Volumes.BgmVolume;
        SeVolume = data.OptionData.Volumes.SeVolume;
        VoiceVolume = data.OptionData.Volumes.VoiceVolume;
        IsMasterMute = data.OptionData.Volumes.IsMasterMute;
        IsBgmMute = data.OptionData.Volumes.IsBgmMute;
        IsSeMute = data.OptionData.Volumes.IsSeMute;
        IsVoiceMute = data.OptionData.Volumes.IsVoiceMute;
    }
}
