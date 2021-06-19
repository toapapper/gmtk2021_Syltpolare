using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace Celezt.UnityEvent
{
    public class OnSetActive : MonoBehaviour
    {
        [SerializeField] GameObject _gameObjectToSet;
        [SerializeField, Tooltip("If game object should be active or not.")] bool _setActive;
        [SerializeField, Tooltip("If it should switch the state each time called.")] bool _isSwitch;

        private bool _isTrue;

        public void OnSetActiveGameObject()
        {
            if (_isSwitch)
            {
                _isTrue = !_isTrue;
                _gameObjectToSet.SetActive(_isTrue);
            }
            else
                _gameObjectToSet.SetActive(_setActive);
        }

        private void OnEnable()
        {
            _isTrue = _setActive;
        }
    }
}
