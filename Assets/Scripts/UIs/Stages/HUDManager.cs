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

    enum HUDViewType
    {
        ON,
        OFF
    }
    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Joystick1Button8))
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
}
