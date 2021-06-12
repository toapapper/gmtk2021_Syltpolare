using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnpoint;
    [SerializeField]
    private GameObject onLight;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Robot")
        {
            this.transform.parent.gameObject.GetComponent<SpawnerManager>().currentSpawner = this.gameObject;
            onLight.SetActive(true);
        }
    }

    public void SpawnDestroyedRobot(GameObject robotToSpawn)
    {
        GameObject spawnedRobot = (GameObject)Instantiate(robotToSpawn, spawnpoint.transform.position, Quaternion.identity);
        this.transform.parent.gameObject.GetComponent<SpawnerManager>().robots.Add(spawnedRobot);
    }
}
