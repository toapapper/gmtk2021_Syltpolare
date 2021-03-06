using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class SpawnPoint : MonoBehaviour
{
    #region Globals
    public static SpawnPoint CurrentSpawnPoint => _spawnPointHistory.LastOrDefault();
    public static int HistoryCount => _spawnPointHistory.Count;
    public static int SpawnPointCount => _spawnPoints.Count;
    public static GameObject LastGameObjectSpawned => _lastGameObjectSpawned;

    public static IEnumerator GetEnumerator() => _spawnPointHistory.GetEnumerator();

    public delegate void SpawnHandler(GameObject spawned);
    public static event SpawnHandler SpawnEvent = delegate { };

    /// <summary>
    /// Spawn new game object at spawn location.
    /// </summary>
    /// <param name="gameObject">Game Object to copy.</param>
    public static void Spawn(GameObject gameObject)
    {
        if (CurrentSpawnPoint != null)
        {
            Transform transform = CurrentSpawnPoint._spawn;
            _lastGameObjectSpawned = Instantiate(gameObject, transform.position, transform.rotation);
            SpawnEvent.Invoke(_lastGameObjectSpawned);
        }
        else
            Debug.LogWarning("No active spawn point was found");
    }

    private static List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private static List<SpawnPoint> _spawnPointHistory = new List<SpawnPoint>();
    private static GameObject _lastGameObjectSpawned;
    #endregion

    /// <summary>
    /// If the spawn point has been activated. Only one spawn point can be activated at once.
    /// </summary>
    public bool IsSpawnPointActivated => _isActivated;

    [SerializeField, Tag] private string _activationTag;
    [SerializeField] private UnityEvent _firstTimeActivateEvent;
    [SerializeField] private UnityEvent _activateEvent;
    [SerializeField] private UnityEvent _deactivateEvent;

    private Transform _spawn;

    private bool _isActivated;
    private bool _isFirstTimeActivated;

    /// <summary>
    /// Activate a spawn point. Deactivate any previous activated.
    /// </summary>
    public void Activate()
    {
        if (!_isActivated)
        {
            _isActivated = true;                                // Activate this spawn point.

            if (HistoryCount > 0)
            {
                CurrentSpawnPoint._isActivated = false;            // Deactivate the previous spawn point.
                CurrentSpawnPoint._deactivateEvent.Invoke();
            }

            _spawnPointHistory.Add(this);
            _activateEvent.Invoke();

            if (_isFirstTimeActivated)                          // If it is the first time it has been activated.
            {
                _isFirstTimeActivated = false;
                _firstTimeActivateEvent.Invoke();
            }
        }
    }

    private void Awake()
    {
        _spawn = transform.Find("Spawn");
    }

    private void OnEnable()
    {
        _isFirstTimeActivated = true;
        _spawnPoints.Add(this);
    }

    private void OnDisable()
    {
        _isActivated = false;
        _spawnPoints.Remove(this);
        _spawnPointHistory.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _activationTag)
            Activate();
    }
}
