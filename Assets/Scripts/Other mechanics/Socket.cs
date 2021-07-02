using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

[RequireComponent(typeof(SpriteRenderer))]
public class Socket : MonoBehaviour
{
    List<Plug> plugsNearby = new List<Plug>();
    public float pluggedInDistance = .2f;

    public float attractionForce = 600f;
    public bool powered = true;//bara kollas av andra saker, mer beh�ver inte g�ras i detta scriptet

    [ReadOnly] public bool occupied = false;
    [ReadOnly] public Plug occupiedBy;

    [SerializeField] private UnityEvent plugEvent;
    [SerializeField] private UnityEvent unplugEvent;

    [HideInInspector] public Circuit circuit = null;

    protected SpriteRenderer sr;
    protected Color UnpoweredColor = Color.grey;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (powered)
            sr.color = Color.white;
        else
            sr.color = UnpoweredColor;

        List<int> removeAt = new List<int>();
        for(int i = 0; i < plugsNearby.Count; i++)
        {
            Plug currentPlug = plugsNearby[i];
            //rensa listan fr�n plugs som har 'Destroy'ats
            if (currentPlug == null)
            {
                removeAt.Add(i);
                continue;
            }

            if (currentPlug.held)
                continue;

            if (!occupied)
            {
                //attrahera plug:
                currentPlug.Attract(transform.position, attractionForce);
                if (Vector2.Distance(transform.position, currentPlug.transform.position) < pluggedInDistance)
                    PlugIn(currentPlug);
            }
        }
        foreach (int i in removeAt)//rensa
            plugsNearby.RemoveAt(i);
        removeAt.Clear();


        if (occupiedBy == null)
        {
            occupied = false;
        }
        if (occupied == false)
        {
            occupiedBy = null;
        }
    }

    public void SetCircuit(Circuit circuit)
    {
        this.circuit = circuit;
    }

    protected void PlugIn(Plug plug)
    {
        if (plug.PlugIn(this))
        {
            occupied = true;
            occupiedBy = plug;
            if(circuit != null)
                circuit.SocketPluggedIn();

            plugEvent.Invoke();
        }
    }

    public void Unplugg(Plug plug)
    {
        if (occupiedBy == plug)
        {
            occupiedBy = null;
            if (circuit != null)
                circuit.SocketUnplugged();

            unplugEvent.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//h�ll reda p� plugs inom range, om denna inte �r upptagen attrahera dem.
    {
        if (collision.CompareTag("Plug"))
        {
            Plug collPlug = collision.gameObject.GetComponent<Plug>();
            if (!plugsNearby.Contains(collPlug))
            {
                plugsNearby.Add(collPlug);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plug"))
        {
            Plug collPlug = collision.gameObject.GetComponent<Plug>();
            if (plugsNearby.Contains(collPlug))
            {
                plugsNearby.Remove(collPlug);
            }
        }
    }

}
