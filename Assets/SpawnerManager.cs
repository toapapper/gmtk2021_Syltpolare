using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> robots;
    public List<GameObject> robotTypes;
    public List<GameObject> unlockedRobots;
    GameObject[] allInScene;
    float poweredRobots;
    Spawner lastCheckpoint;
    GameObject smallRobot;
    GameObject bigRobot;
    GameObject reverseRobot;
    float timer = 0;
    float time = 5;
    public SceneLoaderAndController sceneLoaderAndController;

    public GameObject currentSpawner;
    List<Spawner> listOfSpawners;
    void Start()
    {
        listOfSpawners = new List<Spawner>();
        unlockedRobots = new List<GameObject>();

        foreach (Transform child in transform)
        {
            listOfSpawners.Add(child.gameObject.GetComponent<Spawner>());
        }
        unlockedRobots = robotTypes;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawner != null)
        {
            lastCheckpoint = currentSpawner.GetComponent<Spawner>();
        }
        allInScene = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject item in allInScene)
        {
            if (!robots.Contains(item))
            {
                robots.Add(item);
            }

        }

        poweredRobots = Possess.Count;
        if (robots != null)
        {
            if (poweredRobots == 0)
            {
                if (timer <= 0)
                    timer = time;
                else if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        Debug.Log("ENDGAME");
                        sceneLoaderAndController.RestartScene();
                    }
                }
            }
            else if (poweredRobots != 0 && timer > 0)
                timer = 0;
            
        } 

        if (currentSpawner != null)
        {
            if (robotTypes[0] != null && unlockedRobots.Contains(robotTypes[0]) && robots[0] == null)
            {
                CallSpawner(robotTypes[0], 0);
            }
            if (robotTypes[2] != null && unlockedRobots.Contains(robotTypes[1]) && robots[1] == null)
            {
                CallSpawner(robotTypes[1], 1);
            }
            //if (robotTypes[3] != null && unlockedRobots.Contains(robotTypes[3]) && !robots.Contains(robotTypes[3]))
            //{
            //    CallSpawner(robotTypes[3]);
            //}
        }
    }
    void CallSpawner(GameObject botToSpawn, int spawnIndex)
    {
        lastCheckpoint.SpawnDestroyedRobot(botToSpawn, spawnIndex);
    }
}
