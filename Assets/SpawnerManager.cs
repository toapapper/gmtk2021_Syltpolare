using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> robots = new List<GameObject>();
    public List<GameObject> unlockedRobots;
    GameObject[] allInScene;
    float poweredRobots;
    Spawner lastCheckpoint;
    public GameObject smallRobot;
    public GameObject bigRobot;
    public GameObject reverseRobot;
    float timer = 0;
    float time = 5;
    public SceneLoaderAndController sceneLoaderAndController;
    bool hasStarted = false;
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
        unlockedRobots.Add(smallRobot);
        unlockedRobots.Add(bigRobot);
        unlockedRobots.Add(reverseRobot);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawner != null)
        {
            lastCheckpoint = currentSpawner.GetComponent<Spawner>();
        }
        allInScene = GameObject.FindGameObjectsWithTag("Player");
        if (hasStarted == false)
        {
            robots.Add(smallRobot);
            robots.Add(smallRobot);
            robots.Add(smallRobot);
            hasStarted = true;
        }
        foreach (GameObject item in allInScene)
        {
            if (item.GetComponent<RobotValues>().type == "BigRobot")
            {
                robots[0] = item;
            }
            if (item.GetComponent<RobotValues>().type == "SmallRobot")
            {
                robots[1] = item;
            }
            if (item.GetComponent<RobotValues>().type == "ReverseRobot")
            {
                robots[2] = item;
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
            if (unlockedRobots.Contains(smallRobot) && robots[1] == null)
            {
                CallSpawner(smallRobot, 1);
            }
            if (unlockedRobots.Contains(bigRobot) && robots[0] == null)
            {
                CallSpawner(bigRobot, 0);
            }
            if (unlockedRobots.Contains(reverseRobot) && robots[2] == null)
            {
                CallSpawner(reverseRobot, 2);
            }
        }
    }
    void CallSpawner(GameObject botToSpawn, int spawnIndex)
    {
        lastCheckpoint.SpawnDestroyedRobot(botToSpawn, spawnIndex);
    }
}
