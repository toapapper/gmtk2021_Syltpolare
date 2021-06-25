using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Create Input")]
    public class OnCreateInput : MonoBehaviour
    {
        [SerializeField] private float _value = 0;
        [SerializeField] private InputContext.InputState _state = InputContext.InputState.Canceled;
        [SerializeField] private UnityEvent<InputContext> _inputContextEvent;

        public void OnCreate() => _inputContextEvent.Invoke(new InputContext(_value, _state));
    }
}

