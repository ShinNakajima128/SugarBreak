using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] 
    int _maxHp = 8;

    [SerializeField] 
    int _hp = 8;

    [SerializeField] 
    int _totalKonpeitou = 0;

    [SerializeField]
    WeaponList _weaponList = default;

    [SerializeField]
    Stage[] _stages = default;

    #region property
    public int MaxHp
    {
        get { return _maxHp; }
        set 
        { 
            if (_maxHp <= 10) _maxHp = value;
            
            if (_maxHp > 10)
            {
                _maxHp = 10;
            }
        }
    }

    public int HP
    {
        get 
        { 
            return _hp; 
        }
        set 
        { 
            if (_hp <= _maxHp) 
            {
                _hp = value;
            }

            if (_hp > _maxHp)
            {
                _hp = _maxHp;
            }

            if (_hp <= 0)
            {
                _hp = 0;
            }
        }
    }

    /// <summary>
    /// 金平糖の所持数
    /// </summary>
    public int TotalKonpeitou
    {
        get { return _totalKonpeitou; }
        set 
        { 
            _totalKonpeitou = value;
            //所持数を更新
            EventManager.OnEvent(Events.GetKonpeitou);
        }
    }

    /// <summary>
    /// 現在装備している武器リスト
    /// </summary>
    public WeaponList CurrentWeaponList { get => _weaponList; set => _weaponList = value; }

    public Stage[] StageData { get => _stages; set => _stages = value; }
#endregion

    public void SetStartHp(int hp)
    {
        _maxHp = hp;
        _hp = hp;
    }
    public void SetData(GameData data)
    {
        _maxHp = data.PlayerData.MaxHp;
        _totalKonpeitou = data.PlayerData.TotalKonpeitou;
        _weaponList = data.PlayerData.CurrentWeaponList;
        _stages = data.PlayerData.Stages;
    }
}
