using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Switch Game Object")]
    public class OnSwitchGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject _firstGameObject;
        [SerializeField] private GameObject _secondGameObject;

        private bool _isFirst = true;

        public void OnSwitchGameObjectChange()
        {
            _isFirst = !_isFirst;

            if (_isFirst)
            {
                _firstGameObject.SetActive(true);
                _secondGameObject.SetActive(false);
            }
            else
            {
                _firstGameObject.SetActive(false);
                _secondGameObject.SetActive(true);
            }

        }

        private void OnEnable()
        {
            _isFirst = true;
            _firstGameObject.SetActive(true);
            _secondGameObject.SetActive(false);
        }
    }
}
