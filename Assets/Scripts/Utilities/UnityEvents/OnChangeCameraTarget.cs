using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Change Camera Target")]
    public class OnChangeCameraTarget : MonoBehaviour
    {
        [SerializeField] Transform _target;

        private CameraManager _cameraManager;

        public void OnChangeTarget()
        {
            _cameraManager.Clear();
            _cameraManager.AddMember(_target);
        }

        private void Start()
        {
            _cameraManager = Camera.main.GetComponent<CameraManager>();
        }
    }
}
