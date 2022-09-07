using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpMat : MonoBehaviour
{
    [SerializeField] float m_jumpPower = 10.0f;

    [SerializeField]
    float _animValue = 0.3f;

    Transform _matTrans;
    Vector3 _originScale;

    void Start()
    {
        _matTrans = GetComponent<Transform>();
        _originScale = _matTrans.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Rigidbody>();
            collision.gameObject.GetComponent<PlayerController>().JumpMotion();
            player.AddForce(player.transform.up * m_jumpPower, ForceMode.Impulse);
            AudioManager.PlaySE(SEType.FieldObject_JumpMat);
            MatAnimation();
            VibrationController.OnVibration(Strength.Low, 0.2f);
        }
    }
    void MatAnimation()
    {
        _matTrans.localScale = _originScale;
        var afterScale = new Vector3(_originScale.x + _animValue, _originScale.y + _animValue, _originScale.z + _animValue);
        _matTrans.DOScale(afterScale, 0.25f)
                   .SetEase(Ease.OutBounce)
                   .OnComplete(() =>
                   {
                       _matTrans.DOScale(_originScale, 0.25f)
                                  .SetEase(Ease.OutBounce);
                   });
    }
}
