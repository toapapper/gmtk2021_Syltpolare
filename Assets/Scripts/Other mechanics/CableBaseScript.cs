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
    private Plug plug;

    private float releaseTimer = 0;
    private float releaseTime = .5f;

    public void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        SpawnCable();
    }

    private void Update()
    {
        if(releaseTimer > 0)
        {
            releaseTimer -= Time.deltaTime;
            plug.attracted = false;
        }
    }

    public void SpawnCable()
    {
        if (cableToSpawn != null)
        {
            spawnedCable = Instantiate(cableToSpawn,transform);
            spawnedCable.transform.Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;//Därför måste den första cableSegment finnas nära plats 0,0 i prefaben
            plug = spawnedCable.transform.Find("plug").GetComponent<Plug>();
        }
        else
            Debug.Log("INGEN KABEL ATT SPAWNA ASSÅ!");
    }
    
    public void ReleaseCable(InputContext context)
    {
        releaseTimer = releaseTime;
        plug.attracted = false;
        plug.UnPlugg();
    } 

    public void OnCableBreak()
    {
        Destroy(spawnedCable);//kan lägga in animation eller nåt sånt här
        SpawnCable();
    }

}
