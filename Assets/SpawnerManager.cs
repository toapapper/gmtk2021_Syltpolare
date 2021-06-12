using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentSpawner;
    List<Spawner> listOfSpawners;
    void Start()
    {
        foreach (GameObject child in transform)
        {
            listOfSpawners.Add(child.GetComponent<Spawner>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
