using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Robot")
        {
            this.transform.parent.gameObject.GetComponent<SpawnerManager>().currentSpawner = this.gameObject;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDestroyedRobot(GameObject robotToSpawn)
    {

    }
}
