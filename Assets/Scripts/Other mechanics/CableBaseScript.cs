using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//spawnar en ny övergångskabelsegment i basen med springjoint. När den når en gräns förvandlas den till ett vanligt kabelsegment

[RequireComponent(typeof(Rigidbody2D))]
public class CableBaseScript : MonoBehaviour
{
    [HideInInspector] public bool powered = false;//sann om en av plugsen e inkopplade i en socket som är powered redan.
    public bool inherentlyPowered = false; //sann om det denna sitter på ska räknas som en kraftkälla

    private GameObject m_player;
    public GameObject cableToSpawn;
    public int cableAmount = 1;
    private GameObject[] spawnedCables;

    private Rigidbody2D myRigidBody;
    private Plug[] plugs;
    protected bool[] plugsRecievingPower;

    protected int[] totalCableSegments;
    protected int[] deadCableSegments;

    private float timeOutDeathTime = 1f;
    protected float[] timeOutDeathTimer;

    public void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        m_player = gameObject;

        spawnedCables = new GameObject[cableAmount];
        totalCableSegments = new int[cableAmount];
        deadCableSegments = new int[cableAmount];
        timeOutDeathTimer = new float[cableAmount];
        plugs = new Plug[cableAmount];
        plugsRecievingPower = new bool[cableAmount];

        SpawnAllCables();
    }

    private void Update()
    {
        for(int i = 0; i<cableAmount; i++)
        {
            if (deadCableSegments == totalCableSegments)
                DeleteCable(i);

            if(timeOutDeathTimer[i] > 0)
            {
                timeOutDeathTimer[i] -= Time.deltaTime;
                //Gör så kabel förstörs helt här.
                if (timeOutDeathTimer[i] <= 0)
                    DeleteCable(i);
            }
        }
        powered = CheckPowered();

        if (!powered && Possess.Contains(m_player))
            Possess.Remove(m_player);
        else if (powered && !Possess.Contains(m_player))
            Possess.Add(m_player);
    }

    public Plug[] getPlugs()
    {
        return plugs;
    }

    protected void SpawnAllCables()
    {
        for (int i = 0; i < cableAmount; i++)
            SpawnCable(i);
    }

    public void SpawnCable(int index)
    {
        if (cableToSpawn != null)
        {
            Vector2 position = transform.position;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (index % 2 == 1)
                rotation = Quaternion.Euler(180, 0, 0);
            GameObject cable = Instantiate(cableToSpawn, position, rotation, transform);

            cable.transform.Find("CableSegment").GetComponent<HingeJoint2D>().connectedBody = myRigidBody;
            plugs[index] = cable.transform.Find("plug").GetComponent<Plug>();
            plugs[index].cableIndex = index;

            //räkna antal cablesegments här
            totalCableSegments[index] = cable.transform.childCount - 1;//-1 för plug(g)en e inte kabel
            CableSegmentScript previousgObj = null;

            for (int i = 0; i < totalCableSegments[index]; i++)
            {
                CableSegmentScript currObj = cable.transform.GetChild(i).gameObject.GetComponent<CableSegmentScript>();
                currObj.cableIndex = index;

                if (i == 0 || currObj == null)
                {
                    previousgObj = currObj;
                    continue;
                }
                else
                {
                    if (previousgObj != null)
                    {
                        previousgObj.nextSegment = currObj;
                        currObj.preceedingSegment = previousgObj;
                    }
                }

                previousgObj = currObj;
            }
            if (previousgObj != null)
            {
                plugs[index].preceedingCableSegment = previousgObj;
                previousgObj.nextPlug = plugs[index];
            }

            spawnedCables[index] = cable;
            deadCableSegments[index] = 0;
            timeOutDeathTimer[index] = 0;
        }
        else
            Debug.Log("INGEN KABEL ATT SPAWNA ASSÅ!");
    }

    public void CableDied(int index)
    {
        if (index > cableAmount - 1)
            return;

        deadCableSegments[index]++;

        if (deadCableSegments[index] >= totalCableSegments[index] + 1)//räkna med plug-en
            DeleteCable(index);
    }

    public void ReleaseCable(InputContext context)
    {
        foreach(Plug plug in plugs)
        {
            if (plug.pluggedIn)
            {
                plug.UnPlugg();
                break;
            }

        }
    }

    protected bool CheckPowered()
    {
        if (inherentlyPowered)
            return true;

        for(int i = 0; i < cableAmount; i++)
        {
            if (plugs[i].pluggedIn && plugs[i].socket.powered)
                return true;
        }

        return false;
    }

    public void DeleteCable(int index)
    {
        if (index > cableAmount - 1)
            return;

        timeOutDeathTimer[index] = 0;
        Destroy(spawnedCables[index]);
        SpawnCable(index);
    }

    public void OnCableBreak(int index)
    {
        if (index > cableAmount - 1)
            return;

        if (timeOutDeathTimer[index] <= 0)
            timeOutDeathTimer[index] = timeOutDeathTime;
    }

}
