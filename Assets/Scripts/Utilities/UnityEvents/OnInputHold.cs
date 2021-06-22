using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Times;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Input Hold")]
    public class OnInputHold : MonoBehaviour
    {
        [SerializeField] private float _time = 2.0f;
        [SerializeField] private UnityEvent _startEvent;
        [SerializeField] private UnityEvent _endEvent;
        [SerializeField] private UnityEvent _cancelEvent;

        private Coroutine _waitUntil;
        private Duration _duration;

        private bool _isDone;

        public void OnHoldActivation(InputContext context)
        {
            if (isActiveAndEnabled)
            {
                switch (context.State)
                {
                    case InputContext.InputState.Performed:
                        _isDone = false;
                        _duration = new Duration(_time, false);
                        _startEvent.Invoke();
                        _waitUntil = StartCoroutine(WaitHold());
                        break;
                    case InputContext.InputState.Canceled:
                        if (_waitUntil != null)
                            StopCoroutine(_waitUntil);
                        if (!_isDone)
                            _cancelEvent.Invoke();
                        break;
                    default:
                        break;
                }
            }
        }

        private IEnumerator WaitHold()
        {
            yield return new WaitUntil(() => !_duration.IsActive);
            _isDone = true;
            _endEvent.Invoke();
        }
    }
}
