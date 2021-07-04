using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Time;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Input Router")]
    public class OnInputRouter : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _delay = 0.0f;
        [SerializeField] private UnityEvent<InputContext> _routeEvent;

        public void Connect(InputContext context)
        {
            if (_delay == 0)
                _routeEvent.Invoke(context);
            else
                StartCoroutine(Delay(context));
        }

        private IEnumerator Delay(InputContext context)
        {
            yield return new WaitForSeconds(_delay);
            _routeEvent.Invoke(context);
        }
    }
}

