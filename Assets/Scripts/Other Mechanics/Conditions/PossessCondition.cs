using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Timeline;

public class PossessCondition : ConditionBehaviour
{
    public override bool Condition => _condition;

    [SerializeField] private UnityEvent _conditionTrueEvent;
    [SerializeField] private UnityEvent _conditionFalseEvent;

    private bool _condition;

    public void OnChangePossessed(IReadOnlyList<Possess> possesses)
    {
        if (!isActiveAndEnabled)
            return;

        GameObject spawned = SpawnPoint.LastGameObjectSpawned;

        if (spawned != null && Possess.GetCurrentPossessed == spawned)
        {
            _condition = true;
            _conditionTrueEvent.Invoke();
        }
        else
        {
            _condition = false;
            _conditionFalseEvent.Invoke();
        }
    }

    private void OnEnable()
    {
        _condition = false;
        _conditionFalseEvent.Invoke();
        Possess.ChangeEvent += OnChangePossessed;
    }

    private void OnDisable()
    {
        Possess.ChangeEvent -= OnChangePossessed;
    }
}
