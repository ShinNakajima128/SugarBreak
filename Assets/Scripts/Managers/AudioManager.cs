using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    /// <summary> UI:選択音 </summary>
    UI_Select,
    /// <summary> UI:キャンセル音 </summary>
    UI_Cancel,
    /// <summary> UI:ロード音 </summary>
    UI_Load,
    /// <summary> UI:カーソル移動 </summary>
    UI_CursolMove,
    /// <summary> UI:画面遷移音 </summary>
    UI_Transition,
    /// <summary> プレイヤー:足音 </summary>
    Player_FootStep,
    /// <summary> プレイヤー:ジャンプ </summary>
    Player_Jump,
    /// <summary> プレイヤー:ダメージ </summary>
    Player_Damage,
    /// <summary> プレイヤー:回復音 </summary>
    Player_Heal,
    /// <summary> プレイヤー:金平糖を獲得 </summary>
    Player_GetItem,
    /// <summary> 武器:武器変更 </summary>
    Weapon_Change,
    /// <summary> 武器:突き </summary>
    Weapon_Thrust,
    /// <summary> 武器:お菓子破棄 </summary>
    Weapon_Discard,
    /// <summary> 武器:振り回し </summary>
    Weapon_Wield,
    /// <summary> 武器:叩きつけ </summary>
    Weapon_Strike,
    /// <summary> 武器:射撃 </summary>
    Weapon_Shoot,
    /// <summary> 武器:爆発 </summary>
    Weapon_Explosion,
    /// <summary> 武器:コンボ攻撃 </summary>
    Weapon_Combo,
    /// <summary> 武器:コンボのフィニッシュ </summary>
    Weapon_Finish,
    /// <summary> 敵全般:被弾 </summary>
    Enemy_Damage,
    /// <summary> 敵全般:消滅 </summary>
    Enemy_Vanish,
    /// <summary> デコリー:移動 </summary>
    Decolly_Move,
    /// <summary> デコリー:攻撃 </summary>
    Decolly_Attack,
    /// <summary> ビターゴーレム:足音 </summary>
    BetterGolem_FootStep,
    /// <summary> ビターゴーレム:登場 </summary>
    BetterGolem_Flap,
    /// <summary> ビターゴーレム:吠える </summary>
    BetterGolem_Roar,
    /// <summary> ビターゴーレム:攻撃 </summary>
    BetterGolem_Attack,
    /// <summary> ビターゴーレム:ダメージ </summary>
    BetterGolem_Damage,
    /// <summary> ビターゴーレム:ダウン </summary>
    BetterGolem_Down,
    /// <summary> ビターゴーレム:爆散 </summary>
    BetterGolem_Dead,
    /// <summary> フィールドオブジェクト:ジャンプマット </summary>
    FieldObject_JumpMat,
    /// <summary> フィールドオブジェクト:壊れる音 </summary>
    FieldObject_Break,
    /// <summary> アイテム:チョコエッグを壊す </summary>
    Item_GetChocoEgg,
    /// <summary> 武器:アイテム装着 </summary>
    Weapon_Attach,
    /// <summary> UI:ボタン選択 </summary>
    UI_ButtonSelect,
}
public enum VOICEType
{
    /// <summary> 通常攻撃 </summary>
    Attack_Normal,
    /// <summary> 叩きつけ攻撃 </summary>
    Attack_Strike,
    /// <summary> コンボ1段目 </summary>
    Attack_Combo_First,
    /// <summary> コンボ2段目 </summary>
    Attack_Combo_Second,
    /// <summary> コンボフィニッシュ </summary>
    Attack_Finish,
    /// <summary> 回避 </summary>
    Avoid,
    /// <summary> ステージ外へ落下 </summary>
    FallOffStage,
    /// <summary> 復帰 </summary>
    CameBack,
    /// <summary> ジャンプ </summary>
    Jump,
    /// <summary> ダメージ </summary>
    Damage,
    /// <summary> ゲームオーバー </summary>
    Gameover
}
/// <summary>
/// オーディオ機能を管理するコンポーネント
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("各音量")]
    [SerializeField, Range(0f, 1f)]
    float _masterVolume = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float _bgmVolume = 0.3f;

    [SerializeField, Range(0f, 1f)]
    float _seVolume = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float _voiceVolume = 1.0f;

    [Header("AudioSourceの生成数")]
    [Tooltip("SEのAudioSourceの生成数")]
    [SerializeField]
    int _seAudioSourceNum = 5;

    [Tooltip("SEのAudioSourceの生成数")]
    [SerializeField]
    int _voiceAudioSourceNum = 5;

    [Header("各音源リスト")]
    [SerializeField]
    List<BGM> _bgmList = new List<BGM>();

    [SerializeField]
    List<SE> _seList = new List<SE>();

    [SerializeField]
    List<VOICE> _voiceList = new List<VOICE>();

    [Header("使用する各オブジェクト")]
    [Tooltip("BGM用のAudioSource")]
    [SerializeField]
    AudioSource _bgmSource = default;

    [Tooltip("SE用のAudioSourceをまとめるオブジェクト")]
    [SerializeField]
    Transform _seSourcesParent = default;

    [Tooltip("ボイス用のAudioSourceをまとめるオブジェクト")]
    [SerializeField]
    Transform _voiceSourcesParent = default;

    [Tooltip("AudioMixer")]
    [SerializeField]
    AudioMixerGroup _mixer = default;

    List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    List<AudioSource> _voiceAudioSourceList = new List<AudioSource>();

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //指定した数のSE用AudioSourceを生成
        for (int i = 0; i < _seAudioSourceNum; i++)
        {
            //SEAudioSourceのオブジェクトを生成し、親オブジェクトにセット
            var obj = new GameObject($"SESource{i + 1}");
            obj.transform.SetParent(_seSourcesParent);

            //生成したオブジェクトにAudioSourceを追加
            var source = obj.AddComponent<AudioSource>();
            
            _seAudioSourceList.Add(source);
        }

        //指定した数のボイス用AudioSourceを生成
        for (int i = 0; i < _voiceAudioSourceNum; i++)
        {
            //VOICEAudioSourceのオブジェクトを生成し、親オブジェクトにセット
            var obj = new GameObject($"VOICESource{i + 1}");
            obj.transform.SetParent(_voiceSourcesParent);

            //生成したオブジェクトにAudioSourceを追加
            var source = obj.AddComponent<AudioSource>();

            _voiceAudioSourceList.Add(source);
        }
    }

    #region play method
    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="type"> BGMの種類 </param>
    public static void PlayBGM(BGMType type, bool loopType = true)
    {
        var bgm = GetBGM(type);

        if (bgm != null)
        {
            if (Instance._bgmSource.clip == null)
            {
                Instance._bgmSource.clip = bgm.Clip;
                Instance._bgmSource.loop = loopType;
                Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume;
                Instance._bgmSource.Play();
                Debug.Log($"{bgm.BGMName}を再生");

            }
            else
            {
                Instance.StartCoroutine(Instance.SwitchingBgm(bgm));
            }

        }
        else
        {
            Debug.LogError($"BGM:{type}を再生できませんでした");
        }

    }

    /// <summary>
    /// SEを再生
    /// </summary>
    /// <param name="type"> SEの種類 </param>
    public static void PlaySE(SEType type)
    {
        var se = GetSE(type);

        if (se != null)
        {
            foreach (var s in Instance._seAudioSourceList)
            {
                if (!s.isPlaying)
                {
                    s.PlayOneShot(se.Clip, Instance._seVolume * Instance._masterVolume);
                    Debug.Log($"{se.SEName}を再生");
                    return;
                }
            }
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
        var voice = GetVOICE(type);

        if (voice != null)
        {
            foreach (var s in Instance._voiceAudioSourceList)
            {
                if (!s.isPlaying)
                {
                    s.PlayOneShot(voice.Clip, Instance._voiceVolume * Instance._masterVolume);
                    Debug.Log($"{voice.VOICEName}を再生");
                    return;
                }
            }
        }
        else
        {
            Debug.LogError($"VOICE:{type}を再生できませんでした");
        }
    }
    #endregion

    #region stop method
    /// <summary>
    /// 再生中のBGMを停止する
    /// </summary>
    public static void StopBGM()
    {
        Instance._bgmSource.Stop();
        Instance._bgmSource.clip = null;
    }
    /// <summary>
    /// 再生中のSEを停止する
    /// </summary>
    public static void StopSE()
    {
        foreach (var s in Instance._seAudioSourceList)
        {
            s.Stop();
            s.clip = null;
        }
    }
    /// <summary>
    /// 再生中のボイスを停止する
    /// </summary>
    public void StopVOICE()
    {
        foreach (var s in Instance._voiceAudioSourceList)
        {
            s.Stop();
            s.clip = null;
        }
    }
    #endregion
    #region volume Method
    /// <summary>
    /// マスター音量を変更する
    /// </summary>
    /// <param name="masterValue"> 音量 </param>
    public void MasterVolChange(float masterValue)
    {
        _masterVolume = masterValue;
    }

    /// <summary>
    /// BGM音量を変更する
    /// </summary>
    /// <param name="bgmValue"> 音量 </param>
    public void BgmVolChange(float bgmValue)
    {
        _bgmVolume = bgmValue;
    }

    /// <summary>
    /// SE音量を変更する
    /// </summary>
    /// <param name="seValue"> 音量 </param>
    public void SeVolChange(float seValue)
    {
        _seVolume = seValue;
    }

    /// <summary>
    /// ボイス音量を変更する
    /// </summary>
    /// <param name="voiceValue"> 音量 </param>
    public void VoiceVolChange(float voiceValue)
    {
        _voiceVolume = voiceValue;
    }

    /// <summary>
    /// 各音量をセットする
    /// </summary>
    /// <param name="data"> サウンドデータ </param>
    public static void SetVolume(SoundOption data)
    {
        Instance._masterVolume = data.MasterVolume;
        Instance._bgmVolume = data.BgmVolume;
        Instance._seVolume = data.SeVolume;
        Instance._voiceVolume = data.VoiceVolume;
    }

    /// <summary>
    /// BGMを徐々に変更する
    /// </summary>
    /// <param name="afterBgm"> 変更後のBGM </param>
    IEnumerator SwitchingBgm(BGM afterBgm)
    {
        float currentVol = _bgmSource.volume;

        while (_bgmSource.volume > 0)　//現在の音量を0にする
        {
            _bgmSource.volume -= 0.01f * 0.5f;
            yield return null;
        }

        _bgmSource.clip = afterBgm.Clip;　//BGMの入れ替え
        _bgmSource.Play();

        while (_bgmSource.volume < currentVol)　//音量を元に戻す
        {
            _bgmSource.volume += 0.01f * 0.5f;
            yield return null;
        }
        _bgmSource.volume = currentVol;

    }
    #endregion

    #region get method
    /// <summary>
    /// BGMを取得
    /// </summary>
    /// <param name="type"> BGMの種類 </param>
    /// <returns> 指定したBGM </returns>
    static BGM GetBGM(BGMType type)
    {
        var bgm = Instance._bgmList.FirstOrDefault(b => b.BGMType == type);
        return bgm;
    }
    /// <summary>
    /// SEを取得
    /// </summary>
    /// <param name="type"> SEの種類 </param>
    /// <returns> 指定したSE </returns>
    static SE GetSE(SEType type)
    {
        var se = Instance._seList.FirstOrDefault(s => s.SEType == type);
        return se;
    }
    /// <summary>
    /// ボイスを取得
    /// </summary>
    /// <param name="type"> ボイスの種類 </param>
    /// <returns> 指定したボイス </returns>
    static VOICE GetVOICE(VOICEType type)
    {
        var voice = Instance._voiceList.FirstOrDefault(v => v.VOICEType == type);
        return voice;
    }
    #endregion
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
