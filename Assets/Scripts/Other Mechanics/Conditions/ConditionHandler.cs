using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Timeline;

public class ConditionHandler : ConditionBehaviour
{
    public override bool Condition => _condition;

    [SerializeField] private bool _startCondition;
    [SerializeField] private UnityEvent _conditionTrueEvent;
    [SerializeField] private UnityEvent _conditionFalseEvent;

    private bool _condition;

    public void OnConditionTrue()
    {
        if (!isActiveAndEnabled)
            return;

        _condition = true;
        _conditionTrueEvent.Invoke();
    }
    public void OnConditionFalse()
    {
        if (!isActiveAndEnabled)
            return;

        _condition = false;
        _conditionFalseEvent.Invoke();
    }

    private void OnEnable()
    {
        _condition = _startCondition;
        _conditionFalseEvent.Invoke();
    }
}
