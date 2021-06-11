using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FloatEvent _horizontalMovement;

    private bool _isHorizontalDown;

    private void Update()
    {
        float horizontalValue = Input.GetAxisRaw(InputData.InputHorizontal);

        if (horizontalValue != 0 && !_isHorizontalDown)
        {
            _horizontalMovement.Invoke(horizontalValue);
            _isHorizontalDown = true;
        }
        else if (horizontalValue == 0 && _isHorizontalDown)
        {
            _horizontalMovement.Invoke(horizontalValue);
            _isHorizontalDown = false;
        }
    }

    [Serializable]
    private class FloatEvent: UnityEvent<float> { }
}
