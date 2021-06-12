using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Possess : MonoBehaviour
{
    public static List<GameObject> PossessableRobots = new List<GameObject>();
    public static int CurrentIndex = 1;

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
        PossessableRobots.Add(gameObject);
    }

    private void OnDisable()
    {
        PossessableRobots.Remove(gameObject);
    }
}
