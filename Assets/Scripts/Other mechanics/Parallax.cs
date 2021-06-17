using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxEffectHorizontal = 1;
    [SerializeField] private float _parallaxEffectVertical = 1;
    [SerializeField] private bool _usePixelPerfect;
    [SerializeField, ConditionalField(nameof(_usePixelPerfect)), Min(0)] private int _pixelPerUnit = 16;

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
        Vector3 newPosition = new Vector3(_startPositon.x + distanceHorizontal, _startPositon.y + distanceVertical, position.z);
        transform.position = _usePixelPerfect ? PixelPerfectClamp(newPosition, _pixelPerUnit) : newPosition;

        if (tempHorizontal > _startPositon.x + _length.x)
            _startPositon.x += _length.x;
        else if (tempHorizontal < _startPositon.x - _length.x)
            _startPositon.x -= _length.x;
    }

    private static Vector3 PixelPerfectClamp(Vector3 locationVector, int pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}
