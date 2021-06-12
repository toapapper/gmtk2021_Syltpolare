using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//spawnar en ny övergångskabelsegment i basen med springjoint. När den når en gräns förvandlas den till ett vanligt kabelsegment

[RequireComponent(typeof(Rigidbody2D))]
public class CableBaseScript : MonoBehaviour
{
    public GameObject cableToSpawn;
    private GameObject spawnedCable;

    private Rigidbody2D myRigidBody;

    public void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        SpawnCable();
    }

    public void SpawnCable()
    {
        if (cableToSpawn != null)
        {
            spawnedCable = Instantiate(cableToSpawn,transform);
            transform.Find(spawnedCable.name).Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;//Därför måste den första cableSegment finnas nära plats 0,0 i prefaben
        }
        else
            Debug.Log("INGEN KABEL ATT SPAWNA ASSÅ!");
    }

    public void OnCableBreak()
    {
        Destroy(spawnedCable);//kan lägga in animation eller nåt sånt här
        SpawnCable();
    }

}
