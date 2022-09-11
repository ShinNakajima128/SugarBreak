using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    GameObject _hudPanel = default;

    [SerializeField]
    GameObject _weaponIconPanel = default;

    [SerializeField]
    HUDViewType _currentViewType = HUDViewType.ON;

    public static HUDManager Instance { get; private set; }

    enum HUDViewType
    {
        ON,
        OFF
    }
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Joystick1Button5))
            .Subscribe(_ => 
            {
                switch (_currentViewType)
                {
                    case HUDViewType.ON:
                        _hudPanel.SetActive(false);
                        _weaponIconPanel.SetActive(false);
                        _currentViewType = HUDViewType.OFF;
                        break;
                    case HUDViewType.OFF:
                        _hudPanel.SetActive(true);
                        _weaponIconPanel.SetActive(true);
                        _currentViewType = HUDViewType.ON;
                        break;
                    default:
                        break;
                }
            });
    }
    public void OnHUD()
    {
        _hudPanel.SetActive(true);
        _weaponIconPanel.SetActive(true);
        _currentViewType = HUDViewType.ON;
    }

    public void OffHUD()
    {
        _hudPanel.SetActive(false);
        _weaponIconPanel.SetActive(false);
        _currentViewType = HUDViewType.OFF;
    }
}
