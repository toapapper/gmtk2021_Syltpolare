using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using MyBox;

[RequireComponent(typeof(Light2D))]
public class LightFlicker2D : MonoBehaviour
{
    public bool StopFlickering
    {
        get => _stopFlickering;
        set => _stopFlickering = value;
    }

    [SerializeField, MinMaxRange(0, 1)] private MinMaxFloat _intesityRange = new MinMaxFloat(0, 1);
    [SerializeField, Min(0)] private float _rateDamping = 0.01f;
    [SerializeField] private float _speed = 100.0f;
    [SerializeField] private bool _stopFlickering;

    private Light2D _lightSource;
    private float _baseIntensity;
    private bool _flickering;

    public void Reset()
    {
        _intesityRange = new MinMaxFloat(0, 1);
        _rateDamping = 0.01f;
        _speed = 100.0f;
    }

    public void Start()
    {
        _lightSource = GetComponent<Light2D>();
        _baseIntensity = _lightSource.intensity;
        StartCoroutine(DoFlicker());
    }

    void Update()
    {
        if (!_stopFlickering && !_flickering)
        {
            StartCoroutine(DoFlicker());
        }
    }

    private IEnumerator DoFlicker()
    {
        _flickering = true;
        while (!_stopFlickering)
        {
            float randomRange = Random.Range(_intesityRange.Min, _intesityRange.Max);
            _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, _baseIntensity * randomRange, _speed * Time.deltaTime);
            yield return new WaitForSeconds(_rateDamping);
        }
        _flickering = false;
    }
}
