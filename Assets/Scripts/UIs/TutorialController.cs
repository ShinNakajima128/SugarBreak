using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _operationPanels = default;

    [SerializeField]
    TutorialPanelType _currentPanelType = TutorialPanelType.None;

    [SerializeField]
    Image _background = default;

    [SerializeField]
    Sprite[] _backgroundSprite = default;

    [SerializeField]
    GameObject _tutorialPanel = default;
    public static TutorialController Instance { get; private set; }

    enum TutorialPanelType
    {
        None,
        Basic,
        MainWeapon,
        CandyBeat,
        PopLauncher,
        DualSoda
    }

    void Awake()
    {
        Instance = this;
    }

    public void OnPanel(int type)
    {
        var t = (TutorialPanelType)type;

        //if (_currentPanelType == t && t != TutorialPanelType.None)
        //{
        //    return;
        //}

        foreach (var p in _operationPanels)
        {
            p.SetActive(false);
        }

        _tutorialPanel.SetActive(true);
        _currentPanelType = t;

        switch (_currentPanelType)
        {
            case TutorialPanelType.None:
                break;
            case TutorialPanelType.Basic:
                _operationPanels[0].SetActive(true);
                break;
            case TutorialPanelType.MainWeapon:
                _operationPanels[1].SetActive(true);
                break;
            case TutorialPanelType.CandyBeat:
                _operationPanels[2].SetActive(true);
                break;
            case TutorialPanelType.PopLauncher:
                _operationPanels[3].SetActive(true);
                break;
            case TutorialPanelType.DualSoda:
                _operationPanels[4].SetActive(true);
                break;
            default:
                break;
        }
        if (_currentPanelType == TutorialPanelType.None || _currentPanelType == TutorialPanelType.Basic)
        {
            _background.sprite = _backgroundSprite[0];
        }
        else
        {
            _background.sprite = _backgroundSprite[1];
        }
    }
    public void OnPanel(WeaponTypes type)
    {
        _tutorialPanel.SetActive(true);

        foreach (var p in _operationPanels)
        {
            p.SetActive(false);
        }

        switch (type)
        {
            case WeaponTypes.None:
                break;
            case WeaponTypes.MainWeapon:
                _operationPanels[1].SetActive(true);
                _currentPanelType = TutorialPanelType.MainWeapon;
                ButtonUIController.Instance.OnCurrentPanelFirstButton(4);
                break;
            case WeaponTypes.CandyBeat:
                _operationPanels[2].SetActive(true);
                _currentPanelType = TutorialPanelType.CandyBeat;
                ButtonUIController.Instance.OnCurrentPanelFirstButton(5);
                break;
            case WeaponTypes.PopLauncher:
                _operationPanels[3].SetActive(true);
                _currentPanelType = TutorialPanelType.PopLauncher;
                ButtonUIController.Instance.OnCurrentPanelFirstButton(6);
                break;
            case WeaponTypes.DualSoda:
                _operationPanels[4].SetActive(true);
                _currentPanelType = TutorialPanelType.DualSoda;
                ButtonUIController.Instance.OnCurrentPanelFirstButton(7);
                break;
            default:
                break;
        }
        if (_currentPanelType == TutorialPanelType.None || _currentPanelType == TutorialPanelType.Basic)
        {
            _background.sprite = _backgroundSprite[0];
        }
        else
        {
            _background.sprite = _backgroundSprite[1];
        }
    }
    public void OffPanel()
    {
        OnPanel(0);
        _tutorialPanel.SetActive(false);
    }
    public void SelectSE()
    {
        AudioManager.PlaySE(SEType.UI_CursolMove);
    }
}
