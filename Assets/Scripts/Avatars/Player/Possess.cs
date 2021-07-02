using System;
using System.Linq;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Possess : MonoBehaviour
{
    #region Global
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
        if (_possessableRobots.Contains(gameObject))
            return;

        if (!gameObject.TryGetComponent(out Possess possess))
            return;

        _possessableRobots.Add(gameObject);
        _possessComponents.Add(possess);

        if (Count == 1) // Switch to that possessable if it is the only one in the scene.
        {
            _currentPossessed = gameObject;
            _cameraManager.AddMember(gameObject.transform);
            _currentPossessed.GetComponent<Possess>()._changePossessedEvent.Invoke();
            _currentPossessed.GetComponentInHierarchy<PlayerController>()?.SetEnabled(true);
            _currentPossessed.GetComponent<PlayerMovement>()?.SetEnabled(true);

            ChangeEvent.Invoke(_possessComponents.AsReadOnly());
        }

        AddEvent.Invoke(_possessComponents.AsReadOnly());
    }

    /// <summary>
    /// Remove possessable. Changes to another possessable if the current one is removed.
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Remove(GameObject gameObject)
    {
        if (!gameObject.TryGetComponent(out Possess possess))
            return;

        if (gameObject == _currentPossessed)
        {
            if (Count == 1)
            {
                Clear();
                _currentPossessed = null;
                _cameraManager.Clear();
                possess._changePossessedEvent.Invoke();

                return;
            }

            SwitchPossessed(gameObject);
        }


        _possessableRobots.Remove(gameObject);
        _possessComponents.Remove(possess);

        RemoveEvent.Invoke(_possessComponents.AsReadOnly());
    }

    public delegate void PossessHandler(IReadOnlyList<Possess> possesses);
    public static event PossessHandler AddEvent = delegate { };
    public static event PossessHandler RemoveEvent = delegate { };
    public static event PossessHandler ChangeEvent = delegate { };

    private static List<GameObject> _possessableRobots = new List<GameObject>();
    private static List<Possess> _possessComponents = new List<Possess>();
    private static CameraManager _cameraManager;
    private static GameObject _currentPossessed;
    #endregion

    [SerializeField] private UnityEvent _changePossessedEvent;

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
        gameObject.GetComponentInHierarchy<PlayerController>().enabled = false;
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
            _currentPossessed.GetComponentInHierarchy<PlayerController>().enabled = true;      // Activate possessed's controller.
            _currentPossessed.GetComponent<PlayerMovement>().enabled = true;
            gameObject.GetComponentInHierarchy<PlayerController>().enabled = false;            // Disable current controller.
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            _cameraManager.Clear();
            _cameraManager.AddMember(_currentPossessed.transform);              // Add possessed to camera targets.

            for (int i = 0; i < Count; i++)
            {
                _possessComponents[i]._changePossessedEvent.Invoke();
            }
        }

        ChangeEvent.Invoke(_possessComponents.AsReadOnly());
    }

    private static void Clear()
    {
        for (int i = 0; i < _possessableRobots.Count; i++)
        {
            _possessableRobots[i].GetComponentInHierarchy<PlayerController>()?.SetEnabled(false);
            _possessableRobots[i].GetComponent<PlayerMovement>()?.SetEnabled(false);
        }


        _possessableRobots.Clear();
        _possessComponents.Clear();

        RemoveEvent.Invoke(_possessComponents.AsReadOnly());
    }
}
