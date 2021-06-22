using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;
using Celezt.Times;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Input Switch")]
    public class OnInputSwitch : MonoBehaviour
    {
        [SerializeField] private InputContext.InputState _state = InputContext.InputState.Performed;
        [SerializeField, ConditionalField(nameof(_state), false, InputContext.InputState.Canceled)] float _cancelTime = 2.0f;
        [SerializeField, Tooltip("Set Active by default.")] private bool _setOn;
        [SerializeField] private UnityEvent _onEvent;
        [SerializeField] private UnityEvent _offEvent;

        private Duration _duration;

        private bool _isOn;

        public void OnSetSwitch(bool isOn) => _isOn = isOn;

        public void OnInputSwitchChange(InputContext context)
        {
            if (_state == InputContext.InputState.Canceled)
            {
                switch (context.State)
                {
                    case InputContext.InputState.Performed:
                        _duration = new Duration(_cancelTime);
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
                    _onEvent.Invoke();
                else
                    _offEvent.Invoke();
            }
        }

        private void OnEnable()
        {
            _isOn = _setOn;

            if (_isOn)
                _onEvent.Invoke();
            else
                _offEvent.Invoke();
        }
    }
}
