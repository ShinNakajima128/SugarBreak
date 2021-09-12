using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BaseBGState
{
    Default,
    StageSelect,
    Item,
    Weapon,
    Option,
    Tutorial,
}
public class BaseBGController : MonoBehaviour
{
    [Header("各項目の背景")]
    [SerializeField]
    GameObject[] m_images = default;

    [Header("各項目の背景の位置")]
    [SerializeField]
    GameObject[] m_changePositions = default;

    [Header("背景が切り替わる速度")]
    [SerializeField]
    float m_changeSpeed = 0.3f;

    BaseBGState m_baseBGState;

    Vector3 m_currentPos;

    void Start()
    {
        m_currentPos = m_images[0].transform.position;
        BaseStateDefault();
    }

    public void BaseStateDefault()
    {
        ChangeImageByState(BaseBGState.Default);

    }

    public void BaseStateStageSelect()
    {
        ChangeImageByState(BaseBGState.StageSelect);
    }

    public void BaseStateItem()
    {
        ChangeImageByState(BaseBGState.Item);
    }
    public void BaseStateWeapon()
    {
        ChangeImageByState(BaseBGState.Weapon);
    }

    public void BaseStateOption()
    {
        ChangeImageByState(BaseBGState.Option);
    }

    public void BaseStateTutorial()
    {
        ChangeImageByState(BaseBGState.Tutorial);
    }

    void ChangeImageByState(BaseBGState state)
    {
        m_baseBGState = state;

        switch (m_baseBGState)
        {
            case BaseBGState.Default:
                ChangeImage(0);
                SmoothChangePosition(0);
                break;
            case BaseBGState.StageSelect:
                ChangeImage(1);
                SmoothChangePosition(1);
                break;
            case BaseBGState.Item:
                ChangeImage(2);
                SmoothChangePosition(2);
                break;
            case BaseBGState.Weapon:
                ChangeImage(3);
                SmoothChangePosition(3);
                break;
            case BaseBGState.Option:
                ChangeImage(4);
                SmoothChangePosition(4);

                break;
            case BaseBGState.Tutorial:
                ChangeImage(5);
                SmoothChangePosition(5);
                break;
        }
    }

    void ChangeImage(int index)
    {
        for (int i = 0; i < m_images.Length; i++)
        {
            if (i == index)
            {
                m_images[i].SetActive(true);
                m_images[i].transform.position = m_currentPos;
            }
            else
            {
                m_images[i].SetActive(false);
            }
        }
    }

    void SmoothChangePosition(int index)
    {
        m_images[index].GetComponent<RectTransform>()
                       .DOMove(m_changePositions[index].transform.position, m_changeSpeed)
                       .OnComplete(() =>
                       {
                           m_currentPos = m_images[index].transform.position;
                       });
    }
}
