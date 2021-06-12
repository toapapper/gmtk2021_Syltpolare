using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> robots;
    public List<GameObject> robotTypes;
    List<GameObject> unlockedRobots;
    List<GameObject> poweredRobots;
    Spawner lastCheckpoint;
    GameObject smallRobot;
    GameObject bigRobot;
    GameObject reverseRobot;

    public GameObject currentSpawner;
    List<Spawner> listOfSpawners;
    void Start()
    {
        listOfSpawners = new List<Spawner>();
        robotTypes = new List<GameObject>();
        poweredRobots = new List<GameObject>();
        unlockedRobots = new List<GameObject>();

        foreach (Transform child in transform)
        {
            listOfSpawners.Add(child.gameObject.GetComponent<Spawner>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (robots != null && poweredRobots == null)
        {
            if (robots.Count <= 0 || poweredRobots.Count == 0)
            {
                //End Game
            }
        } 

        if (lastCheckpoint != null)
        {
            if (robotTypes[1] != null && unlockedRobots.Contains(robotTypes[1]) && !robots.Contains(robotTypes[1]))
            {
                CallSpawner(smallRobot);
            }
            if (robotTypes[2] != null && unlockedRobots.Contains(robotTypes[2]) && !robots.Contains(robotTypes[2]))
            {
                CallSpawner(bigRobot);
            }
            if (robotTypes[3] != null && unlockedRobots.Contains(robotTypes[3]) && !robots.Contains(robotTypes[3]))
            {
                CallSpawner(reverseRobot);
            }
        }
    }
    void CallSpawner(GameObject botToSpawn)
    {
        lastCheckpoint.SpawnDestroyedRobot(botToSpawn);
    }
}
