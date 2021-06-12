using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _target;
    [SerializeField] private Color _possessColor;
    [SerializeField] private Color _notPossessColor;

    public void OnEyeColorChange(GameObject possessedObject)
    {
        if (possessedObject == transform.parent.gameObject)
            _target.color = _possessColor;
        else
            _target.color = _notPossessColor;
    }
}
