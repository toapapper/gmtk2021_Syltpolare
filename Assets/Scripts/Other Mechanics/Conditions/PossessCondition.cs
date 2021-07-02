using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Timeline;

public class PossessCondition : ConditionBehaviour
{
    public override bool Condition => _condition;

    private bool _condition;

    public void OnChangePossessed(IReadOnlyList<Possess> possesses)
    {
        GameObject spawned = SpawnPoint.LastGameObjectSpawned;

        if (spawned != null && Possess.GetCurrentPossessed == spawned)
            _condition = true;
        else
            _condition = false;
    }

    private void OnEnable()
    {
        _condition = false;
        Possess.ChangeEvent += OnChangePossessed;
    }

    private void OnDisable()
    {
        Possess.ChangeEvent -= OnChangePossessed;
    }
}
