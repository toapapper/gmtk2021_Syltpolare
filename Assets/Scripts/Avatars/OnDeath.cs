using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDeath : MonoBehaviour
{
    [SerializeField] private PrefabReference _prefab;

    public UnityEvent PermaDeathEvent;
    public UnityEvent RespawnableDeathEvent;

    public void PermaDeathInvoke() => PermaDeathEvent.Invoke();
    public void RespawnableDeathInvoke() => RespawnableDeathEvent.Invoke();

    public void SpawnOnLast()
    {
        SpawnPoint.Spawn(_prefab.Value);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
