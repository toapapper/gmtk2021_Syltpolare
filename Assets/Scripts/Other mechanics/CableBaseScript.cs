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

    private float timeOutDeathTime = 1f;
    public float timeOutDeathTimer = 0;

    public void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        SpawnCable();
    }

    private void Update()
    {

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
            spawnedCable.transform.Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;
            plug = spawnedCable.transform.Find("plug").GetComponent<Plug>();

            //räkna antal cablesegments här
            totalCableSegments = spawnedCable.transform.childCount - 1;//-1 för plug(g)en e inte kabel
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
            if (previousgObj != null)
            {
                plug.preceedingCableSegment = previousgObj;
                previousgObj.nextPlug = plug;
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

        if (deadCableSegments >= totalCableSegments + 1)//räkna med plug-en
            DeleteCable();
    }

    public void ReleaseCable(InputContext context)
    {
        plug.Release();
    } 
    

    public void DeleteCable()
    {
        timeOutDeathTimer = 0;
        Destroy(spawnedCable);
        SpawnCable();
    }

    public void OnCableBreak()
    {
        if(timeOutDeathTimer <= 0)
            timeOutDeathTimer = timeOutDeathTime;
    }

}
