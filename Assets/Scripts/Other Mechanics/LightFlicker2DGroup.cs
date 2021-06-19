using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using MyBox;

public class LightFlicker2DGroup : MonoBehaviour
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
    [SerializeField] private Light2D[] _lightSources;

    private float[] _baseIntensities;
    private bool _flickering;

    public void Reset()
    {
        _intesityRange = new MinMaxFloat(0, 1);
        _rateDamping = 0.01f;
        _speed = 100.0f;
    }

    public void Start()
    {
        _baseIntensities = new float[_lightSources.Length];
        for (int i = 0; i < _lightSources.Length; i++)
            _baseIntensities[i] = _lightSources[i].intensity;

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
            for (int i = 0; i < _lightSources.Length; i++)
                _lightSources[i].intensity = Mathf.Lerp(_lightSources[i].intensity, _baseIntensities[i] * randomRange, _speed * Time.deltaTime);

            yield return new WaitForSeconds(_rateDamping);
        }
        _flickering = false;
    }
}
