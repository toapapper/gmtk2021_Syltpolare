using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Parallax : MonoBehaviour
{
    [SerializeField] private int _pixelUnit = 100;
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
        transform.position = new Vector3(RoundToPixels(_startPositon.x + distanceHorizontal, _pixelUnit), RoundToPixels(_startPositon.y + distanceVertical, _pixelUnit), position.z);
        if (tempHorizontal > _startPositon.x + _length.x)
            _startPositon.x += _length.x;
        else if (tempHorizontal < _startPositon.x - _length.x)
            _startPositon.x -= _length.x;
    }

    private float RoundToPixels(float value, int unit) => Mathf.Round(value* unit) / unit;

    private Vector2 RoundToPixels(Vector2 vector, int unit)
    {
        vector.x = Mathf.Round(vector.x * unit) / unit;
        vector.y = Mathf.Round(vector.y * unit) / unit;
        return vector;
    }
}
