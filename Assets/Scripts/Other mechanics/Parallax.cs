using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxEffect;

    private Transform _mainCameraTransform;

    private float _length;
    private float _startPositon;

    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;

        _startPositon = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        float temp = _mainCameraTransform.position.x * (1 - _parallaxEffect);
        float distance = _mainCameraTransform.position.x * _parallaxEffect;

        Vector3 position = transform.position;
        transform.position = new Vector3(_startPositon + distance, position.y, position.z);

        if (temp > _startPositon + _length)
            _startPositon += _length;
        else if (temp < _startPositon - _length)
            _startPositon -= _length;
    }
}
