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
        [SerializeField] private UnityEvent _activationEvent;

        private Coroutine _waitUntil;
        private Duration _duration;

        public void SetHoldActivation(InputContext context)
        {
            switch (context.State)
            {
                case InputContext.InputState.Performed:
                    _duration = new Duration(_time);
                    _waitUntil = StartCoroutine(WaitHold());
                    break;
                case InputContext.InputState.Canceled:
                    StopCoroutine(_waitUntil);
                    break;
                default:
                    break;
            }
        }

        private IEnumerator WaitHold()
        {
            yield return new WaitUntil(() => !_duration.IsActive);
            _activationEvent.Invoke();
        }
    }
}
