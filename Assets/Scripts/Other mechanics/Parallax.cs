using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Parallax : MonoBehaviour
{
    [SerializeField] private bool _loopBackground = true;
    [SerializeField] private float _parallaxEffectHorizontal = 1.0f;
    [SerializeField] private float _parallaxEffectVertical = 1.0f;

    private Transform _mainCameraTransform;

    private Vector2 _length;
    private Vector2 _startPositon;

    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;

        _startPositon = transform.position;
        _length = GetComponent<SpriteRenderer>().bounds.size;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = _mainCameraTransform.position;
        float tempHorizontal = cameraPosition.x * (1 - _parallaxEffectHorizontal);
        float distanceHorizontal = cameraPosition.x * _parallaxEffectHorizontal;
        float distanceVertical = cameraPosition.y * _parallaxEffectVertical;

        Vector3 position = transform.position;
        transform.position = new Vector3(_startPositon.x + distanceHorizontal, _startPositon.y + distanceVertical, position.z);

        if (_loopBackground)
        {
            if (tempHorizontal > _startPositon.x + _length.x)
                _startPositon.x += _length.x;
            else if (tempHorizontal < _startPositon.x - _length.x)
                _startPositon.x -= _length.x;
        }
    }
}
