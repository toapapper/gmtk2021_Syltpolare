using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Possess : MonoBehaviour
{
    public static List<GameObject> PossessableRobots = new List<GameObject>();
    public static int CurrentIndex = 1;
    public static CinemachineTargetGroup _targetGroup;

    public void OnPossess(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                CurrentIndex %= PossessableRobots.Count;
                GameObject obj = PossessableRobots[CurrentIndex++];

                if (obj != gameObject)
                {
                    obj.GetComponent<PlayerController>().enabled = true;    // Activate possessed's controller.
                    this.GetComponent<PlayerController>().enabled = false;  // Disable current controller.
                    _targetGroup.RemoveMember(transform);                   // Remove current from camera targets.
                    _targetGroup.AddMember(obj.transform, 1, 0);            // Add possessed to camera targets.
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
