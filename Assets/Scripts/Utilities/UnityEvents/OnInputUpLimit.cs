using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Time;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Input Up Limit")]
    public class OnInputUpLimit : MonoBehaviour
    {
        [SerializeField] float _cancelTime = 2.0f;
        [SerializeField] private UnityEvent<InputContext> _startEvent;
        [SerializeField] private UnityEvent<InputContext> _performEvent;
        [SerializeField] private UnityEvent<InputContext> _cancelEvent;

        private Duration _duration;

        public void OnInputUpLimitActivate(InputContext context)
        {
            if (isActiveAndEnabled)
            {
                switch (context.State)
                {
                    case InputContext.InputState.Performed:
                        _startEvent.Invoke(context);
                        _duration = new Duration(_cancelTime, false);
                        break;
                    case InputContext.InputState.Canceled:
                        if (_duration.IsActive)
                        {
                            _performEvent.Invoke(context);
                        }
                        else
                        {
                            _cancelEvent.Invoke(context);
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}