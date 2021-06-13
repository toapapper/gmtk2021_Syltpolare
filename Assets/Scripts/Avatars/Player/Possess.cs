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
        _possessableRobots.Add(gameObject);

        if (Count == 1) // Switch to that possessable if it is the only one in the scene.
        {
            _currentPossessed = gameObject;
            _targetGroup.AddMember(gameObject.transform, 1, 0);
            _currentPossessed.GetComponent<Possess>()._changePossessedEvent.Invoke();
            _currentPossessed.GetComponent<PlayerController>().enabled = true;
            SwitchPossessed(gameObject);
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
                RemoveAll();
                _currentPossessed = null;
                gameObject.GetComponent<Possess>()._changePossessedEvent.Invoke();

                return;
            }

            SwitchPossessed(gameObject);
        }

        _possessableRobots.Remove(gameObject);
    }

    [SerializeField] private UnityEvent _changePossessedEvent;

    private static List<GameObject> _possessableRobots = new List<GameObject>();
    private static CinemachineTargetGroup _targetGroup;
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
        if (_targetGroup == null)
            _targetGroup = GameObject.Find("CM Follow").GetComponent<CinemachineTargetGroup>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerController>().enabled = false;
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
            gameObject.GetComponent<PlayerController>().enabled = false;            // Disable current controller.
            _targetGroup.RemoveMember(gameObject.transform);                        // Remove current from camera targets.
            _targetGroup.AddMember(_currentPossessed.transform, 1, 0);              // Add possessed to camera targets.

            for (int i = 0; i < Count; i++)
            {
                _possessableRobots[i].GetComponent<Possess>()._changePossessedEvent.Invoke();
            }
        }
    }

    private static void RemoveAll()
    {
        for (int i = 0; i < _possessableRobots.Count; i++)
        {
            _possessableRobots[i].GetComponent<PlayerController>().enabled = false;
        }

        _possessableRobots.Clear();
        _targetGroup.RemoveMember(_currentPossessed.transform);
    }
}
