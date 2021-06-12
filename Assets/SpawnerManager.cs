using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> robots;
    public List<GameObject> robotTypes;
    List<GameObject> unlockedRobots;
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
        robotTypes = new List<GameObject>();
        unlockedRobots = new List<GameObject>();

        foreach (Transform child in transform)
        {
            listOfSpawners.Add(child.gameObject.GetComponent<Spawner>());
        }
    }

    // Update is called once per frame
    void Update()
    {
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
