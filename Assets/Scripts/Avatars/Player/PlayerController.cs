using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UnityEvent<InputContext> _moveHorizontalEvent;
    [SerializeField] private UnityEvent<InputContext> _jumpEvent;
    [SerializeField] private UnityEvent<InputContext> _switchEvent;

    [SerializeField] private UnityEvent<InputContext> _pickupEvent;
    [SerializeField] private UnityEvent<InputContext> _throwEvent;
    [SerializeField] private UnityEvent<InputContext> _releasePlugEvent;
    [SerializeField] private UnityEvent<InputContext> _dropPlugEvent;


    private float _oldHorizontalValue;

    private void Update()
    {
        if (Time.timeScale <= 0)    // Prevent using player controls if time scale is zero.
            return;

        float horizontalValue = Input.GetAxisRaw(InputControls.Horizontal);

        if (horizontalValue != _oldHorizontalValue)
        {
            _moveHorizontalEvent.Invoke(new InputContext(horizontalValue, InputContext.InputState.Performed));
        }

        _oldHorizontalValue = horizontalValue;


        if (Input.GetButtonDown(InputControls.Jump))
        {
            _jumpEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetButtonUp(InputControls.Jump))
        {
            _jumpEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }

        if (Input.GetButtonDown(InputControls.Switch))
        {
            _switchEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetButtonUp(InputControls.Switch))
        {
            _switchEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }

        if (Input.GetMouseButtonDown(0))
        {
            _pickupEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _pickupEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }

        if(Input.GetMouseButtonDown(1))
        {
            _throwEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _throwEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }

        if (Input.GetButtonDown(InputControls.Release))
        {
            _releasePlugEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetButtonUp(InputControls.Release))
        {
            _releasePlugEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }

        if (Input.GetButtonDown(InputControls.Drop))
        {
            _dropPlugEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetButtonUp(InputControls.Drop))
        {
            _dropPlugEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }
    }

    private void OnEnable()
    {
        float horizontalValue = Input.GetAxisRaw(InputControls.Horizontal);
        _moveHorizontalEvent.Invoke(new InputContext(horizontalValue, InputContext.InputState.Canceled));
    }

    private void OnDisable()
    {
        _moveHorizontalEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        _jumpEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        _pickupEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        _throwEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        _dropPlugEvent.Invoke(new InputContext(0, InputContext.InputState.Performed));
    }
}
