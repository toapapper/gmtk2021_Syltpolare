using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class Follower : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private AnimationCurve _DampCurve = AnimationCurve.Constant(0, 1, 1);
    [SerializeField] private UnityEvent _endEvent;

    private VertexPath _vertexPath;

    private float _distanceTravelled;
    private float _length;

    private void Start()
    {
        _vertexPath = _pathCreator.path;
        _length = _vertexPath.length;
    }

    private void Update()
    {
        _distanceTravelled += _speed * _DampCurve.Evaluate(_distanceTravelled / _length) * Time.deltaTime;

        transform.position = _vertexPath.GetPointAtDistance(_distanceTravelled);

        if (_distanceTravelled >= _length && _speed > 0)
        {
            _endEvent.Invoke();
            _distanceTravelled %= _length;
        }
        else if (_distanceTravelled <= 0 && _speed < 0)
        {
            _endEvent.Invoke();
            _distanceTravelled = _length;
        }
    }
}
