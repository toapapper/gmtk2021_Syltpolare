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
            transform.Find(spawnedCable.name).Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;//D�rf�r m�ste den f�rsta cableSegment finnas n�ra plats 0,0 i prefaben
        }
        else
            Debug.Log("INGEN KABEL ATT SPAWNA ASS�!");
    }

    public void OnCableBreak()
    {
        Destroy(spawnedCable);//kan l�gga in animation eller n�t s�nt h�r
        SpawnCable();
    }

}
