using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Possess : MonoBehaviour
{
    /// <summary>
    /// The currently possessed object.
    /// </summary>
    public static GameObject GetCurrentPossessed => _currentPossessed;

    public static void Add(GameObject gameObject)
    {
        _possessableRobots.Add(gameObject);
    }

    public static void Remove(GameObject gameObject)
    {
        _possessableRobots.Remove(gameObject);
    }

    private static List<GameObject> _possessableRobots = new List<GameObject>();
    private static int _currentIndex = 1;
    private static CinemachineTargetGroup _targetGroup;
    private static GameObject _currentPossessed;

    public void OnPossess(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                _currentIndex %= _possessableRobots.Count;
                _currentPossessed = _possessableRobots[_currentIndex++];

                if (_currentPossessed != gameObject)
                {
                    _currentPossessed.GetComponent<PlayerController>().enabled = true;    // Activate possessed's controller.
                    this.GetComponent<PlayerController>().enabled = false;  // Disable current controller.
                    _targetGroup.RemoveMember(transform);                   // Remove current from camera targets.
                    _targetGroup.AddMember(_currentPossessed.transform, 1, 0);            // Add possessed to camera targets.
                }

                break;
            default:
                break;
        }
    }

    private void Start()
    {
        if (_possessableRobots.Count > 0)
            _possessableRobots[0].GetComponent<PlayerController>().enabled = true;
    }

    private void OnEnable()
    {
        if (_possessableRobots.Count == 0)
        {
            _currentPossessed = gameObject;
            _targetGroup = GameObject.Find("CM Follow").GetComponent<CinemachineTargetGroup>();
            _targetGroup.AddMember(transform, 1, 0);
        }

        _possessableRobots.Add(gameObject);
    }

    private void OnDisable()
    {
        _possessableRobots.Remove(gameObject);
    }
}
