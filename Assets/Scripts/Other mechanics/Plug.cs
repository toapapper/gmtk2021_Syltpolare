using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    private GameObject m_player;

    public Vector3 Destination;
    protected bool attractable = true;
    public bool drawingPower = false;//sann om drar kraft ifr?n en socket. Om sann s? kan den inte samtidigt ge kraft till den socketen.
    public bool held;
    public bool pluggedIn;
    public Socket socket { get; protected set; } = null;
    Rigidbody2D _rigidbody;

    public int cableIndex = -1;

    public CableBaseScript cableBase { get; protected set; }

    Animator animator;

    private float releaseTime = 2f;//sekunder som den inte kan attraheras till en socket efter den "releasats" med X
    private float releaseTimer = 0;

    private float animationLength = .667f;
    private float animTimer = 0;

    private float killNextTime = .05f;
    private float killNextTimer = 0;

    public bool dieing = false;

    public CableSegmentScript preceedingCableSegment;

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        cableBase = transform.GetComponentInParent<CableBaseScript>();
        animator = GetComponent<Animator>();
        animator.enabled = false;

        m_player = transform.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (releaseTimer > 0)
        {
            releaseTimer -= Time.deltaTime;
            attractable = false;
        }
        else
            attractable = true;

        //explosions-animation och s?nt
        if(killNextTimer > 0)
        {
            killNextTimer -= Time.deltaTime;
            if (killNextTimer <= 0 && preceedingCableSegment != null && !preceedingCableSegment.dieing)
                preceedingCableSegment.Kill();
        }
        if (animTimer > 0)
        {
            animTimer -= Time.deltaTime;

            if (animTimer <= 0)
            {
                cableBase.CableDied(cableIndex);
                Destroy(gameObject, .01f);
            }
        }

        if (held == true && pluggedIn)
        {
            UnPlugg();
        }
    }

    public void Release()
    {
        releaseTimer = releaseTime;
        UnPlugg();
    }

    public void Kill()
    {
        animTimer = animationLength;
        dieing = true;
        animator.enabled = true;

        if (pluggedIn)
            UnPlugg();

        killNextTimer = killNextTime;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Dangerous"))
        {
            Kill();
            cableBase.OnCableBreak(cableIndex);
        }
    }

    public void Attract(Vector2 attractor, float attractionForce)
    {
        if (attractable && !held)
        {
            Vector2 dirVector = attractor - (Vector2)transform.position;
            _rigidbody.AddForce(dirVector.normalized * attractionForce);
        }
    }

    public void UnPlugg()
    {
        pluggedIn = false;
        if (socket != null)
            socket.Unplugg(this);
        socket = null;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    public bool PlugIn(Socket socket)
    {
        if (attractable && !held)
        {
            this.socket = socket;
            pluggedIn = true;
            _rigidbody.MovePosition(socket.transform.position);
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
            
            return true;
        }
        else
            return false;
    }
    
}
