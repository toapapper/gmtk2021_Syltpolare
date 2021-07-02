using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Timeline;
using MyBox;

[RequireComponent(typeof(Collider2D))]
public class AreaTriggerCondition : ConditionBehaviour
{
    public override bool Condition => _condition;

    [SerializeField, Tag] private string _tag;
    [SerializeField, Tooltip("First since OnEnter. Will also activate enter.")] private UnityEvent _firstEnterEvent;
    [SerializeField] private UnityEvent _enterEvent;
    [SerializeField, Tooltip("First since OnEnter. Will also activate exit.")] private UnityEvent _firstExitEvent;
    [SerializeField] private UnityEvent _exitEvent;

    private bool _condition;
    private bool _isFirstEnter = true;
    private bool _isFirstExit = true;

    private int _insideTrigger;

    private void OnEnable()
    {
        _isFirstEnter = true;
        _isFirstExit = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _tag)
        {
            _insideTrigger++;

            if (_insideTrigger >= 1)
            {
                _condition = true;

                if (_isFirstEnter)
                {
                    _isFirstEnter = false;
                    _firstEnterEvent.Invoke();
                }

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

                if (_isFirstExit)
                {
                    _isFirstExit = false;
                    _firstExitEvent.Invoke();
                }

                _exitEvent.Invoke();
            }
        }
    }
}
