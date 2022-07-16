using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    /// <summary> タイトル画面 </summary>
    Title,
    /// <summary> 拠点:メイン </summary>
    Base_Main,
    /// <summary> 拠点:武器選択 </summary>
    Base_WeaponSelect,
    /// <summary> ベイクルバレー:メイン </summary>
    BakeleValley_Main,
    /// <summary> ベイクルバレー:ボス </summary>
    BakeleValley_Boss,
    /// <summary> レインディ雲海:メイン </summary>
    RaindyClouds_Main,
    /// <summary> レインディ雲海:ボス </summary>
    RaindyClouds_Boss,
    /// <summary> デザートリゾート:メイン </summary>
    DessertResort_Main,
    /// <summary> デザートリゾート:ボス </summary>
    DessertResort_Boss,
    /// <summary> グレース雪原:メイン </summary>
    GlaseSnowField_Main,
    /// <summary> グレース雪原:ボス </summary>
    GlaseSnowField_Boss,
    /// <summary> ガナッシュ火山:メイン </summary>
    GanacheVolcano_Main,
    /// <summary> ガナッシュ火山:ボス </summary>
    GanacheVolcano_Boss,
    /// <summary> ミニゲーム:ブロック壊し </summary>
    MiniGame_BreakingBlocks,
    /// <summary> ステージクリア </summary>
    ClearJingle,
    /// <summary> ゲームオーバー </summary>
    Gameover
}
public enum SEType
{
    
    /// <summary> 選択音 </summary>
    UI_Select,
    /// <summary> キャンセル音 </summary>
    UI_Cancel,
    /// <summary> ロード音 </summary>
    UI_Load,
    /// <summary> 画面遷移音 </summary>
    UI_Transition,
    /// <summary> 足音 </summary>
    Player_FootStep,
    /// <summary> ジャンプ </summary>
    Player_Jump,
    /// <summary> 回復音 </summary>
    Player_Heal,
    /// <summary> 武器変更 </summary>
    Weapon_Change,
    /// <summary> 突き </summary>
    Weapon_Thrust,
    /// <summary> 振り回し </summary>
    Weapon_Wield,
    /// <summary> 叩きつけ </summary>
    Weapon_Strike,
    /// <summary> 射撃 </summary>
    Weapon_Shoot,
    /// <summary> 爆発 </summary>
    Weapon_Explosion,
    /// <summary> コンボ攻撃 </summary>
    Weapon_Combo,
    /// <summary> コンボのフィニッシュ </summary>
    Weapon_Finish,
    /// <summary> ジャンプマット </summary>
    FieldObject_JumpMat,
    /// <summary> 壊れる音 </summary>
    FieldObject_Break
}
public enum VOICEType
{

}
/// <summary>
/// オーディオ機能を管理するコンポーネント
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("各音源リスト")]
    [SerializeField]
    List<BGM> _bgmList = new List<BGM>();

    [SerializeField]
    List<SE> _seList = new List<SE>();

    [SerializeField]
    List<VOICE> _voiceList = new List<VOICE>();

    [Header("使用するコンポーネント")]
    [Tooltip("BGM用のAudioSource")]
    [SerializeField]
    AudioSource _bgmSource = default;

    [Tooltip("SE用のAudioSource")]
    [SerializeField]
    AudioSource _seSource = default;

    [Tooltip("ボイス用のAudioSource")]
    [SerializeField]
    AudioSource _voiceSource = default;

    
    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="type"> BGMの種類 </param>
    public static void PlayBGM(BGMType type)
    {
        var bgm = Instance._bgmList.FirstOrDefault(b => b.BGMType == type);

        if (bgm != null)
        {
            Instance._bgmSource.clip = bgm.Clip;
            Instance._bgmSource.loop = true;
            //m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
            Instance._bgmSource.Play();
            Debug.Log($"{bgm.BGMName}を再生");
        }
        else
        {
            Debug.LogError($"BGM:{type}を再生できませんでした");
        }
    }

    public static void PlaySE(SEType type)
    {
        var se = Instance._seList.FirstOrDefault(s => s.SEType == type);

        if (se != null)
        {
            Instance._seSource.clip = se.Clip;
            Instance._seSource.loop = true;
            //m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
            Instance._seSource.Play();
            Debug.Log($"{se.SEName}を再生");
        }
        else
        {
            Debug.LogError($"SE:{type}を再生できませんでした");
        }
    }

    /// <summary>
    /// ボイスを再生
    /// </summary>
    /// <param name="type"> ボイスの種類 </param>
    public static void PlayVOICE(VOICEType type)
    {
        var voice = Instance._voiceList.FirstOrDefault(v => v.VOICEType == type);

        if (voice != null)
        {
            Instance._voiceSource.clip = voice.Clip;
            Instance._voiceSource.loop = true;
            //m_bgmAudioSource.volume = m_bgmVolume * m_masterVolume;
            Instance._voiceSource.Play();
            Debug.Log($"{voice.VOICEName}を再生");
        }
        else
        {
            Debug.LogError($"VOICE:{type}を再生できませんでした");
        }
    }
}

[Serializable]
public class BGM
{
    public string BGMName;
    public BGMType BGMType;
    public AudioClip Clip;
}
[Serializable]
public class SE
{
    public string SEName;
    public SEType SEType;
    public AudioClip Clip;
}
[Serializable]
public class VOICE
{
    public string VOICEName;
    public VOICEType VOICEType;
    public AudioClip Clip;
}
