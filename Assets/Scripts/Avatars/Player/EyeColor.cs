using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _target;
    [SerializeField] private Color _possessColor = Color.white;
    [SerializeField] private Color _notPossessColor = Color.white;
    [SerializeField] private Color _cannotMoveColor = Color.white;
    [SerializeField] private float _massLimit = 159.0f;

    RigidbodyStack.BodyStack _bodyStack;

    public void OnPossessChange()
    {
        ChangeEyeColor();
    }

    public void OnStackChange(RigidbodyStack.BodyStack bodyStack)
    {
        _bodyStack = bodyStack;

        ChangeEyeColor();
    }

    private void ChangeEyeColor()
    {
        if (Possess.GetCurrentPossessed == transform.parent.gameObject)
        {
            if (_bodyStack.StackedMass > _massLimit)
                _target.color = _cannotMoveColor;
            else
                _target.color = _possessColor;
        }
        else
            _target.color = _notPossessColor;
    }
}