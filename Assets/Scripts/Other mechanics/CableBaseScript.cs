using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//spawnar en ny �verg�ngskabelsegment i basen med springjoint. N�r den n�r en gr�ns f�rvandlas den till ett vanligt kabelsegment

[RequireComponent(typeof(Rigidbody2D))]
public class CableBaseScript : MonoBehaviour
{
    public GameObject cableToSpawn;
    private GameObject spawnedCable;

    private Rigidbody2D myRigidBody;
    private Plug plug;

    private float releaseTimer = 0;
    private float releaseTime = .5f;

    private float timeOutDeathTime = 3f;
    private float timeOutDeathTimer = 0;

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

        //kolla ocks� om alla kablar e d�da, g�r s� de �kar r�kningen n�r de d�r

        if(timeOutDeathTimer > 0)
        {
            timeOutDeathTimer -= Time.deltaTime;
            //G�r s� kabel f�rst�rs helt h�r.
        }

    }

    public void SpawnCable()
    {
        if (cableToSpawn != null)
        {
            spawnedCable = Instantiate(cableToSpawn,transform);
            spawnedCable.transform.Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;//D�rf�r m�ste den f�rsta cableSegment finnas n�ra plats 0,0 i prefaben
            plug = spawnedCable.transform.Find("plug").GetComponent<Plug>();
        }
        else
            Debug.Log("INGEN KABEL ATT SPAWNA ASS�!");


        //
    }
    
    public void ReleaseCable(InputContext context)
    {
        releaseTimer = releaseTime;
        plug.attracted = false;
        plug.UnPlugg();
    } 

    public void OnCableBreak()
    {
        timeOutDeathTimer = timeOutDeathTime;

        Destroy(spawnedCable);//kan l�gga in animation eller n�t s�nt h�r
        SpawnCable();
    }

}
