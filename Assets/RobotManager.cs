using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    List<GameObject> robots;
    List<GameObject> unlockedRobots;
    List<GameObject> poweredRobots;
    GameObject lastCheckpoint;
    GameObject smallRobot;
    GameObject bigRobot;
    GameObject reverseRobot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (robots.Count <= 0 || poweredRobots.Count == 0)
        {
            //End Game
        }

        if (lastCheckpoint != null)
        {
            if (smallRobot == null)
            {
                CallSpawner(smallRobot);
            }
            if (bigRobot == null && unlockedRobots.Contains(bigRobot))
            {
                CallSpawner(bigRobot);
            }
            if (reverseRobot == null && unlockedRobots.Contains(reverseRobot))
            {
                CallSpawner(reverseRobot);
            }
        }


    }
    void CallSpawner(GameObject botToSpawn)
    {
        lastCheckpoint.GetComponent<Spawner>().SpawnDestroyedRobot(botToSpawn);
    }  
}
