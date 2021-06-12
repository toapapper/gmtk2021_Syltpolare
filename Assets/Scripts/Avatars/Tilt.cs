using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _rotationScale = 20.0f;
    [SerializeField] float _rotationSpeed = 5.0f;

    private float _horizontalValue;
    private Quaternion _slerpHorizontalQuaternion;

    public void OnTilt(InputContext context)
    {
        _horizontalValue = context.Value;
    }

    private void Update()
    {
        _slerpHorizontalQuaternion = Quaternion.Slerp(_slerpHorizontalQuaternion, Quaternion.Euler(0, 0, _horizontalValue * _rotationScale), Time.deltaTime * _rotationSpeed);
        _targetTransform.localRotation = _slerpHorizontalQuaternion;
    }
}
