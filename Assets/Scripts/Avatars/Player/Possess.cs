using System;
using System.Linq;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class Possess : MonoBehaviour
{
    /// <summary>
    /// The currently possessed object.
    /// </summary>
    public static GameObject GetCurrentPossessed => _currentPossessed;

    public static int Count => _possessableRobots.Count;

    public static bool Contains(GameObject gameObject) => _possessableRobots.Contains(gameObject);

    /// <summary>
    /// Add new possessable.
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Add(GameObject gameObject)
    {
        if (gameObject.GetComponent<Possess>() is null)
            return;

        _possessableRobots.Add(gameObject);

        if (_cameraManager.IsStarted && Count == 1) // Switch to that possessable if it is the only one in the scene.
        {
            _currentPossessed = gameObject;
            _cameraManager.AddMember(gameObject.transform);
            _currentPossessed.GetComponent<Possess>()._changePossessedEvent.Invoke();
            _currentPossessed.GetComponent<PlayerController>().enabled = true;
            _currentPossessed.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    /// <summary>
    /// Remove possessable. Changes to another possessable if the current one is removed.
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Remove(GameObject gameObject)
    {
        if (gameObject == _currentPossessed)
        {
            if (Count == 1)
            {
                Clear();
                _currentPossessed = null;
                _cameraManager.Clear();
                gameObject.GetComponent<Possess>()._changePossessedEvent.Invoke();

                return;
            }

            SwitchPossessed(gameObject);
        }

        _possessableRobots.Remove(gameObject);
    }

    [SerializeField] private UnityEvent _changePossessedEvent;

    private static List<GameObject> _possessableRobots = new List<GameObject>();
    private static CameraManager _cameraManager;
    private static GameObject _currentPossessed;

    public void OnPossess(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                SwitchPossessed(gameObject);
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        _cameraManager = Camera.main.GetComponent<CameraManager>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        _changePossessedEvent.Invoke();

        Add(gameObject);
    }

    private void OnDisable()
    {
        Remove(gameObject);
    }

    private static void SwitchPossessed(GameObject gameObject)
    {
        _currentPossessed = _possessableRobots.TakeWhile(x => x != _currentPossessed).DefaultIfEmpty(_possessableRobots[_possessableRobots.Count - 1]).LastOrDefault();

        if (_currentPossessed != gameObject)
        {
            _currentPossessed.GetComponent<PlayerController>().enabled = true;      // Activate possessed's controller.
            _currentPossessed.GetComponent<PlayerMovement>().enabled = true;
            gameObject.GetComponent<PlayerController>().enabled = false;            // Disable current controller.
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            _cameraManager.Clear();
            _cameraManager.AddMember(_currentPossessed.transform);              // Add possessed to camera targets.

            for (int i = 0; i < Count; i++)
            {
                _possessableRobots[i].GetComponent<Possess>()._changePossessedEvent.Invoke();
            }
        }
    }

    private static void Clear()
    {
        for (int i = 0; i < _possessableRobots.Count; i++)
        {
            _possessableRobots[i].GetComponent<PlayerController>().enabled = false;
            _possessableRobots[i].GetComponent<PlayerMovement>().enabled = false;
        }

        _possessableRobots.Clear();
    }
}
