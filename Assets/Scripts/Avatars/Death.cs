using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
    [SerializeField] private PrefabReference _prefab;

    public UnityEvent PermaDeathEvent;
    public UnityEvent RespawnableDeathEvent;

    public void SpawnOnLast()
    {
        SpawnPoint.Spawn(_prefab.Value, transform.parent);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
