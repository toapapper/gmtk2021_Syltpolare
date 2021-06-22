using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private bool _isRespawnable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.TryGetComponent(out OnDeath onDeath))
        {
            if (_isRespawnable)
                onDeath.RespawnableDeathEvent?.Invoke();
            else
                onDeath.PermaDeathEvent?.Invoke();
        }
    }
}
