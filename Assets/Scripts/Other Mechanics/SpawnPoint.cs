using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class SpawnPoint : MonoBehaviour
{
    #region Global
    public static SpawnPoint LastSpawnPoint => _spawnPointHistory.LastOrDefault();
    public static int HistoryCount => _spawnPointHistory.Count;
    public static int SpawnPointCount => _spawnPoints.Count;

    public static IEnumerator GetEnumerator() => _spawnPointHistory.GetEnumerator();

    public static void Spawn(GameObject gameObject, Transform parent)
    {
        if (LastSpawnPoint != null)
        {
            Transform transform = LastSpawnPoint.transform;
            Instantiate(gameObject, transform.position, transform.rotation, parent);
        }
        else
            Debug.LogWarning("No active spawn point was found");
    }

    private static List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private static List<SpawnPoint> _spawnPointHistory = new List<SpawnPoint>();
    #endregion

    public bool IsSpawnPointActivated
    {
        get => _activated;
        set => _activated = value;
    }

    [SerializeField, Tag] private string _activationTag;
    [SerializeField] private UnityEvent _activationEvent;

    private bool _activated;

    private void OnEnable()
    {
        _spawnPoints.Add(this);
    }

    private void OnDisable()
    {
        _spawnPoints.Remove(this);
        _spawnPointHistory.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _activationTag)
            Activate();
    }

    private void Activate()
    {
        if (!_activated)
        {
            _activated = true;                                  // Activate this spawn point.
            if (HistoryCount > 0)
                LastSpawnPoint.IsSpawnPointActivated = false;    // Deactivate the previous spawn point.
            _spawnPointHistory.Add(this);
            _activationEvent.Invoke();
        }
    }
}
