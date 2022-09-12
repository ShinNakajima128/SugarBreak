using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SugarBreak;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    float _changeInterval = 0.1f;

    [SerializeField]
    Image[] _volumeBarImages = default;

    [SerializeField]
    VolumeBarTypes _currentVolumeType = VolumeBarTypes.None;

    float _masterVolume;
    float _bgmVolume;
    float _seVolume;
    float _voiceVolume;

    bool _isWaiting = false;

    enum VolumeBarTypes
    {
        None,
        Master,
        BGM,
        SE,
        VOICE
    }

    void OnEnable()
    {
        CurrentVolumeSet();
    }

    private void Update()
    {
        if (_isWaiting)
        {
            return;
        }

        if (Input.GetAxis("UIHorizontal") > 0)
        {
            Debug.Log(Input.GetAxis("UIHorizontal"));
            StartCoroutine(VolumeChange(_currentVolumeType, 0.05f));
        }
        else if (Input.GetAxis("UIHorizontal") < 0)
        {
            StartCoroutine(VolumeChange(_currentVolumeType, -0.05f));
        }
    }

    public void ChangeBarType(int type)
    {
        _currentVolumeType = (VolumeBarTypes)type;
        AudioManager.PlaySE(SEType.UI_CursolMove);
    }

    public void UpdateVolumeData()
    {
        var data = DataManager.Instance.GetOptionData;
        data.SoundOptionData.MasterVolume = _masterVolume;
        data.SoundOptionData.BgmVolume = _bgmVolume;
        data.SoundOptionData.SeVolume = _seVolume;
        data.SoundOptionData.VoiceVolume = _voiceVolume;

        SaveManager.Save(DataTypes.Option);
    }

    IEnumerator VolumeChange(VolumeBarTypes type, float value)
    {
        _isWaiting = true;

        switch (type)
        {
            case VolumeBarTypes.None:
                break;
            case VolumeBarTypes.Master:
                
                _masterVolume += value;

                if (_masterVolume > 1)
                {
                    _masterVolume = 1;
                }
                else if (_masterVolume < 0)
                {
                    _masterVolume = 0;
                }

                _volumeBarImages[0].fillAmount = _masterVolume;
                AudioManager.MasterVolChange(_masterVolume);
                Debug.Log($"マスター音量{_masterVolume}");
                break;
            case VolumeBarTypes.BGM:
                
                _bgmVolume += value;

                if (_bgmVolume > 1)
                {
                    _bgmVolume = 1;
                }
                else if (_bgmVolume < 0)
                {
                    _bgmVolume = 0;
                }

                _volumeBarImages[1].fillAmount = _bgmVolume;
                AudioManager.BgmVolChange(_bgmVolume);
                Debug.Log($"BGM音量{_bgmVolume}");
                break;
            case VolumeBarTypes.SE:

                _seVolume += value;
                
                if (_seVolume > 1)
                {
                    _seVolume = 1;
                }
                else if (_seVolume < 0)
                {
                    _seVolume = 0;
                }
                _volumeBarImages[2].fillAmount = _seVolume;
                AudioManager.SeVolChange(_seVolume);
                AudioManager.PlaySE(SEType.UI_ButtonSelect);
                Debug.Log($"SE音量{_seVolume}");
                break;
            case VolumeBarTypes.VOICE:
                
                _voiceVolume += value;
                
                if (_voiceVolume > 1)
                {
                    _voiceVolume = 1;
                }
                else if (_voiceVolume < 0)
                {
                    _voiceVolume = 0;
                }
                _volumeBarImages[3].fillAmount = _voiceVolume;
                AudioManager.VoiceVolChange(_voiceVolume);
                AudioManager.PlayVOICE(VOICEType.Attack_Combo_First);
                Debug.Log($"VOICE音量{_voiceVolume}");
                break;
            default:
                break;
        }
        yield return new WaitForSecondsRealtime(_changeInterval);

        _isWaiting = false;
        Debug.Log("変更可");
    }

    void CurrentVolumeSet()
    {
        var data = DataManager.Instance.GetOptionData;
        _masterVolume = data.SoundOptionData.MasterVolume;
        _bgmVolume = data.SoundOptionData.BgmVolume;
        _seVolume = data.SoundOptionData.SeVolume;
        _voiceVolume = data.SoundOptionData.VoiceVolume;

        _volumeBarImages[0].fillAmount = _masterVolume;
        _volumeBarImages[1].fillAmount = _bgmVolume;
        _volumeBarImages[2].fillAmount = _seVolume;
        _volumeBarImages[3].fillAmount = _voiceVolume;
    }
}
