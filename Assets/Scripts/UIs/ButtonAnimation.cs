using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Sequence seq;
    Image buttonImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        seq?.Kill();

        seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(transform.position.y - 7, 0.05f))
           .Join(buttonImage.DOColor(new Color(0.7f, 0.7f, 0.7f), 0.05f))
           .Append(transform.DOMoveY(transform.position.y, 0.05f))
           .Join(buttonImage.DOColor(new Color(1, 1, 1), 0.05f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        seq?.Kill();

        seq = DOTween.Sequence();

        seq.Append(transform.DOScale(new Vector3(0.45f, 0.45f, 0.45f), 0.5f))
        .Append(transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.5f))
        .SetLoops(-1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        seq?.Kill();
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }
}
