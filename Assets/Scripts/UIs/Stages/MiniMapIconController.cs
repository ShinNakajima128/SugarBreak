using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIconController : MonoBehaviour
{
    [SerializeField]
    RectTransform _playerIcon = default;

    [SerializeField]
    RectTransform _cameraDirIcon = default;

    [SerializeField]
    Transform _playerObj = default;

    [SerializeField]
    Transform _camera = default;

    void Update()
    {
        Vector3 playerRot = new Vector3(0, 0, _playerObj.rotation.y);
        _playerIcon.localRotation = Quaternion.Euler(0, 0, -_playerObj.localEulerAngles.y);
        Vector3 cameraRot = new Vector3(0, 0, Camera.main.transform.rotation.y);
        _cameraDirIcon.localRotation = Quaternion.Euler(0, 0, -_camera.localEulerAngles.y);
    }
}
