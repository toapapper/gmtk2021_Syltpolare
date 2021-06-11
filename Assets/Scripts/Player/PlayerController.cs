using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> _moveHorizontalEvent;
    [SerializeField] private UnityEvent<float> _jumpEvent;

    private bool _isHorizontalDown;
    private bool _isJumpDown;

    private void Update()
    {
        float horizontalValue = Input.GetAxisRaw(InputData.Horizontal);

        if (horizontalValue != 0 && !_isHorizontalDown)
        {
            _moveHorizontalEvent.Invoke(horizontalValue);
            _isHorizontalDown = true;
        }
        else if (horizontalValue == 0 && _isHorizontalDown)
        {
            _moveHorizontalEvent.Invoke(horizontalValue);
            _isHorizontalDown = false;
        }

        if (Input.GetButtonDown(InputData.Jump))
        {
            if (!_isJumpDown)
            {
                _jumpEvent.Invoke(1);
                _isJumpDown = true;
            }

        }

        if (Input.GetButtonUp(InputData.Jump))
        {
            if (_isJumpDown)
            {
                _jumpEvent.Invoke(0);
                _isJumpDown = false;
            }
        }
    }
}
