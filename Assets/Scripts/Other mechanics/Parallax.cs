using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class Parallax : MonoBehaviour
{
    [SerializeField] private bool _loopBackground = true;
    [SerializeField] private float _parallaxEffectHorizontal = 1.0f;
    [SerializeField] private float _parallaxEffectVertical = 1.0f;
    [SerializeField] private PivotPosition _pivot = PivotPosition.Transform;
    [SerializeField, ConditionalField(nameof(_pivot), false, PivotPosition.CustomTransform)] private Transform _targetTransform;

    private Transform _mainCameraTransform;

    private Vector3 _length;
    private Vector3 _pivotPosition;

    private enum PivotPosition
    {
        Transform,
        CustomTransform,
    }

    private void OnEnable()
    {
        _mainCameraTransform = Camera.main.transform;

        switch (_pivot)
        {
            case PivotPosition.Transform:
                _pivotPosition = transform.position;
                break;
            case PivotPosition.CustomTransform:
                _pivotPosition = _targetTransform.position;
                break;
        }

        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            _length = spriteRenderer.bounds.size;
        else if (TryGetComponent(out RectTransform rectTransform))
            _length = rectTransform.GetWorldRect().size;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = _mainCameraTransform.position;
        float tempHorizontal = cameraPosition.x * (1 - _parallaxEffectHorizontal);
        float distanceHorizontal = cameraPosition.x * _parallaxEffectHorizontal;
        float distanceVertical = cameraPosition.y * _parallaxEffectVertical;

        Vector3 position = transform.position;
        transform.position = new Vector3(_pivotPosition.x + distanceHorizontal, _pivotPosition.y + distanceVertical, position.z);

        if (_loopBackground)
        {
            if (tempHorizontal > _pivotPosition.x + _length.x)
                _pivotPosition.x += _length.x;
            else if (tempHorizontal < _pivotPosition.x - _length.x)
                _pivotPosition.x -= _length.x;
        }
    }
}
