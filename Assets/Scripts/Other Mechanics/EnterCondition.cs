using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Timeline;
using MyBox;

[RequireComponent(typeof(Collider2D))]
public class EnterCondition : ConditionBehaviour
{
    public override bool Condition
    {
        get => _condition;
        set => _condition = value;
    }

    [SerializeField, Tag] private string _tag;
    [SerializeField] private UnityEvent _enterEvent;
    [SerializeField] private UnityEvent _exitEvent;

    private bool _condition;

    private int _insideTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _tag)
        {
            _insideTrigger++;

            if (_insideTrigger >= 1)
            {
                _condition = true;
                _enterEvent.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == _tag)
        {
            Mathf.Max(0, --_insideTrigger);

            if (_insideTrigger == 0)
            {
                _condition = false;
                _exitEvent.Invoke();
            }
        }
    }
}
