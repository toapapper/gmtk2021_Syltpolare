using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UnityEvent<InputContext> _moveHorizontalEvent;
    [SerializeField] private UnityEvent<InputContext> _jumpEvent;

    private bool _isHorizontalDown;
    private bool _isJumpDown;

    private void Update()
    {
        float horizontalValue = Input.GetAxisRaw(InputControls.Horizontal);

        if (horizontalValue != 0 && !_isHorizontalDown)
        {
            _moveHorizontalEvent.Invoke(new InputContext(horizontalValue, InputContext.InputState.Performed));
            _isHorizontalDown = true;
        }
        else if (horizontalValue == 0 && _isHorizontalDown)
        {
            _moveHorizontalEvent.Invoke(new InputContext(horizontalValue, InputContext.InputState.Canceled));
            _isHorizontalDown = false;
        }

        if (Input.GetButtonDown(InputControls.Jump))
        {
            if (!_isJumpDown)
            {
                _jumpEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
                _isJumpDown = true;
            }

        }

        if (Input.GetButtonUp(InputControls.Jump))
        {
            if (_isJumpDown)
            {
                _jumpEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
                _isJumpDown = false;
            }
        }
    }
}
