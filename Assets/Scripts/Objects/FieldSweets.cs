using UnityEngine;
using ObjectOutline;
using UniRx;

/// <summary>
/// ステージに存在するオブジェクトのデータを持つクラス
/// </summary>
public class FieldSweets : MonoBehaviour
{
    [SerializeField]
    FieldSweetsData m_sweetsData = default;

    [SerializeField]
    OutlineActivator _outlineAct = default;

    Outline _outline;
    ReactiveProperty<bool> _isEnable = new ReactiveProperty<bool>(false);

    void Start()
    {
        if (TryGetComponent<Outline>(out _outline))
        {
            //Debug.Log("Outlineコンポーネント取得");
        };

        if (_outlineAct != null) 
        {
            _outlineAct.OnReactiveAction += OnOutline;
            
            _outlineAct.OffReactiveAction += OffOutline;
        };
        EventManager.ListenEvents(Events.OnSweetsOutline, Activate);
        EventManager.ListenEvents(Events.OffSweetsOutline, Deactivate);

        _isEnable.Subscribe((x) =>
        {
            if (x)
            {
                OnOutline();
            }
            else
            {
                OffOutline();
            }
        });
    }
    
    public FieldSweetsSize SweetsType => m_sweetsData.SweetsSize;
    public Vector3 ColliderSize => m_sweetsData.ColliderSize;
    public int AttackPower => m_sweetsData.AttackPower;
    public int KonpeitouNum => m_sweetsData.KonpeitouNum;
    public int EnduranceCount => m_sweetsData.EnduranceCount;
    public Outline Outline => _outline;

    void Activate()
    {
        _isEnable.Value = true;
    }

    void Deactivate()
    {
        _isEnable.Value = false;
    }

    void OnOutline()
    {
        if (_outlineAct == null)
        {
            return;
        }

        if (_outline != null && _isEnable.Value && _outlineAct.IsInArea)
        {
            _outline.enabled = true;
        }
    }
    void OffOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }
}
