using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;
using Celezt.Time;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Input Switch")]
    public class OnInputSwitch : MonoBehaviour
    {
        [SerializeField] private InputContext.InputState _state = InputContext.InputState.Performed;
        [SerializeField, ConditionalField(nameof(_state), false, InputContext.InputState.Canceled)] float _cancelTime = 2.0f;
        [SerializeField, Tooltip("Set Active by default.")] private bool _setOn;
        [SerializeField] private UnityEvent<InputContext> _onEvent;
        [SerializeField] private UnityEvent<InputContext> _offEvent;

        private Duration _duration;

        private bool _isOn;

        public void OnSetSwitch(bool isOn) => _isOn = isOn;

        public void OnInputSwitchChange(InputContext context)
        {
            if (isActiveAndEnabled)
            {
                if (_state == InputContext.InputState.Canceled)
                {
                    switch (context.State)
                    {
                        case InputContext.InputState.Performed:
                            _duration = new Duration(_cancelTime, false);
                            break;
                        case InputContext.InputState.Canceled:
                            if (!_duration.IsActive)
                                return;
                            break;
                        default:
                            break;
                    }
                }

                if (context.State == _state)
                {
                    _isOn = !_isOn;

                    if (_isOn)
                        _onEvent.Invoke(context);
                    else
                        _offEvent.Invoke(context);
                }
            }
        }

        private void OnEnable()
        {
            _isOn = _setOn;

            InputContext context = _state == InputContext.InputState.Performed ?
                new InputContext(1, InputContext.InputState.Performed) :
                new InputContext(0, InputContext.InputState.Canceled);

            if (_isOn)
                _onEvent.Invoke(context);
            else
                _offEvent.Invoke(context);
        }
    }
}
