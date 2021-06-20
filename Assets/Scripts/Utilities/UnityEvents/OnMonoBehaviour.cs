using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Mono Behaviour")]
    public class OnMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private UnityEvent _awakeEvent;
        [SerializeField] private UnityEvent _startEvent;
        [SerializeField] private UnityEvent _enableEvent;
        [SerializeField] private UnityEvent _disableEvent;
        [SerializeField] private UnityEvent _destroyEvent;

        private void Awake()
        {
            _awakeEvent.Invoke();
        }

        private void Start()
        {
            _startEvent.Invoke();
        }

        private void OnEnable()
        {
            _enableEvent.Invoke();
        }

        private void OnDisable()
        {
            _disableEvent.Invoke();
        }

        private void OnDestroy()
        {
            _destroyEvent.Invoke();
        }
    }
}
