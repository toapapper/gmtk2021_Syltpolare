using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Celezt.Timeline;
using MyBox;

[RequireComponent(typeof(Collider2D))]
public class EnterCondition : ConditionBehaviour
{
    public override bool Condition => _condition;

    [SerializeField, Tag] private string _tag;

    private bool _condition = true;

    private int _insideTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _tag)
        {
            _insideTrigger++;

            if (_insideTrigger >= 1)
                _condition = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == _tag)
        {
            Mathf.Max(0, --_insideTrigger);

            if (_insideTrigger == 0)
                _condition = true;
        }
    }
}
