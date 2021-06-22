using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Times;
using UnityEngine.UI;
using MyBox;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Image Fill")]
    [RequireComponent(typeof(Image))]
    public class OnImageFill : MonoBehaviour
    {

        [SerializeField] private float _time = 2.0f;
        [SerializeField, MinMaxRange(0, 1)] private MinMaxFloat _fillRange = new MinMaxFloat(0, 1);
        [SerializeField] private UnityEvent _doneEvent;

        private Image _image;
        private Coroutine _fill;

        private float _fillAmount;
        private bool _isFilling;

        public float FillAmount
        {
            get
            {
                return _fillAmount;
            }
            set
            {
                _fillAmount = Mathf.Clamp(value, _fillRange.Min, _fillRange.Max);

                float fillPercentage = _fillAmount / _fillRange.Max;
                _image.fillAmount = fillPercentage;
            }
        }

        public void OnBeginFill()
        {
            if (isActiveAndEnabled)
            {
                FillAmount = 0;
                _isFilling = true;
                _fill = StartCoroutine(Fill());
            }
        }

        public void OnStopFill()
        {
            if (isActiveAndEnabled)
            {
                FillAmount = 0;
                _isFilling = false;
                if (_fill != null)
                    StopCoroutine(_fill);
            }
        }

        public void OnPauseFill()
        {
            _isFilling = false;
        }

        public void OnResumeFill()
        {
            _isFilling = true;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _isFilling = true;
            _fillAmount = 0;
        }

        private IEnumerator Fill()
        {
            while (true)
            {
                if (_isFilling)
                {
                    FillAmount += _fillRange.Length() / _time * Time.unscaledDeltaTime;

                    _image.fillAmount = FillAmount;

                    if (FillAmount <= _fillRange.Min)
                        break;
                    else if (FillAmount >= _fillRange.Max)
                        break;
                }

                yield return null;
            }

            _doneEvent.Invoke();
        }
    }
}
