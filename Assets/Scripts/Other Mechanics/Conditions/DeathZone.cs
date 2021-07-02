using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Timeline;
using MyBox;

public class DeathZone : ConditionBehaviour
{
    public override bool Condition => _condition;

    [SerializeField] private bool _isRespawnable = true;

    private bool _condition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.TryGetComponent(out OnDeath onDeath))
        {
            if (_isRespawnable)
                onDeath.RespawnableDeathEvent?.Invoke();
            else
                onDeath.PermaDeathEvent?.Invoke();

            SendCondition();
        }
    }

    private IEnumerator SendCondition()
    {
        _condition = true;
        yield return null;
        _condition = false;
    }
}
