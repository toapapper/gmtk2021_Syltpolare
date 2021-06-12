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
    public GameObject GetCurrentPossessed => _currentPossessed;

    public static List<GameObject> PossessableRobots = new List<GameObject>();
    private static int _currentIndex = 1;
    private static CinemachineTargetGroup _targetGroup;
    private static GameObject _currentPossessed;

    public void OnPossess(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                _currentIndex %= PossessableRobots.Count;
                _currentPossessed = PossessableRobots[_currentIndex++];

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
        if (PossessableRobots.Count > 0)
            PossessableRobots[0].GetComponent<PlayerController>().enabled = true;
    }

    private void OnEnable()
    {
        if (PossessableRobots.Count == 0)
        {
            _currentPossessed = gameObject;
            _targetGroup = GameObject.Find("CM Follow").GetComponent<CinemachineTargetGroup>();
            _targetGroup.AddMember(transform, 1, 0);
        }

        PossessableRobots.Add(gameObject);
    }

    private void OnDisable()
    {
        PossessableRobots.Remove(gameObject);
    }
}
