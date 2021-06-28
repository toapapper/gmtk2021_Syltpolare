using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private UnityEvent<TriggerState, Collider2D> _groundCheckEvent;

    private int _insideTrigger;

    public enum TriggerState
    {
        Grounded,
        Elevation,
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layerMask.Contains(collision.gameObject.layer))
        {
            _insideTrigger++;

            if (_insideTrigger >= 1)
                _groundCheckEvent.Invoke(TriggerState.Grounded, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_layerMask.Contains(collision.gameObject.layer))
        {
            Mathf.Max(0, --_insideTrigger);

            if (_insideTrigger == 0)
                _groundCheckEvent.Invoke(TriggerState.Elevation, collision);
        }
    }
}