using UnityEngine;

public class PlayerCameraRotate : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;

    [SerializeField] private float _topClamp = 70.0f;
    [SerializeField] private float _bottomClamp = -30.0f;
    [SerializeField] private float _rotateMultiplayer = 1f;

    private float _targetYaw;
    private float _targetPitch;

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");

        _targetYaw += mouseXInput * _rotateMultiplayer;
        _targetPitch += mouseYInput * _rotateMultiplayer;

        _targetYaw = ClampAngle(_targetYaw, float.MinValue, float.MaxValue);
        _targetPitch = ClampAngle(_targetPitch, _bottomClamp, _topClamp);

        _cameraTarget.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
