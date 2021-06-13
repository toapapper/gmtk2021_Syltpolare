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

    public int totalCableSegments;
    public int deadCableSegments;

    private float releaseTimer = 0;
    private float releaseTime = .5f;

    private float timeOutDeathTime = 1f;
    public float timeOutDeathTimer = 0;

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

        //kolla också om alla kablar e döda, gör så de ökar räkningen när de dör

        if (deadCableSegments == totalCableSegments)
            DeleteCable();

        if(timeOutDeathTimer > 0)
        {
            timeOutDeathTimer -= Time.deltaTime;
            //Gör så kabel förstörs helt här.
            if (timeOutDeathTimer <= 0)
                DeleteCable();
        }

    }

    public void SpawnCable()
    {
        if (cableToSpawn != null)
        {
            spawnedCable = Instantiate(cableToSpawn,transform);
            spawnedCable.transform.Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;//Därför måste den första cableSegment finnas nära plats 0,0 i prefaben
            plug = spawnedCable.transform.Find("plug").GetComponent<Plug>();

            //räkna antal cablesegments här
            totalCableSegments = spawnedCable.transform.childCount - 1;//-1 för plugen e inte kabel
            CableSegmentScript previousgObj = null;

            for(int i = 0; i < totalCableSegments; i++)
            {
                CableSegmentScript currObj = spawnedCable.transform.GetChild(i).gameObject.GetComponent<CableSegmentScript>();

                if (i == 0 || currObj == null)
                {
                    previousgObj = currObj;
                    continue;
                }
                else
                {
                    if(previousgObj != null)
                    {
                        previousgObj.nextSegment = currObj;
                        currObj.preceedingSegment = previousgObj;
                    }
                }

                previousgObj = currObj;
            }


            deadCableSegments = 0;
            timeOutDeathTimer = 0;
        }
        else
            Debug.Log("INGEN KABEL ATT SPAWNA ASSÅ!");


        //
    }
    
    public void CableDied()
    {
        Debug.Log("Cable dIed");

        deadCableSegments++;

        if (deadCableSegments >= totalCableSegments)
            DeleteCable();
    }

    public void ReleaseCable(InputContext context)
    {
        releaseTimer = releaseTime;
        plug.attracted = false;
        plug.UnPlugg();
    } 
    

    public void DeleteCable()
    {
        timeOutDeathTimer = 0;
        Destroy(spawnedCable);
        SpawnCable();
    }

    public void OnCableBreak()
    {
        timeOutDeathTimer = timeOutDeathTime;
    }

}
