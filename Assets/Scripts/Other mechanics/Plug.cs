using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public Vector3 Destination;
    public bool held;
    public bool attracted;
    public bool pluggedIn;
    Rigidbody2D _rigidbody;

    CableBaseScript cableBase;

    Animator animator;

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
    }

    // Update is called once per frame
    void Update()
    {
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
                cableBase.CableDied();
                Destroy(gameObject, .01f);
            }
        }

        if (attracted == true && held == false)
        {
            Attracted();
        }
        if (held == true)
        {
            Held();
        }
        if (pluggedIn == true && !Possess.Contains(transform.root.gameObject))
        {
            Possess.Add(transform.root.gameObject);
        }
        else if (pluggedIn == false && Possess.Contains(transform.root.gameObject))
        {
            Possess.Remove(transform.root.gameObject);
        }
    }


    public void Kill()
    {
        animTimer = animationLength;
        dieing = true;
        animator.enabled = true;

        killNextTimer = killNextTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Dangerous"))
        {
            Kill();

            cableBase.OnCableBreak();
        }
    }


    void Attracted()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.MovePosition(Destination - transform.position * Time.deltaTime);
        if (Mathf.Abs(Destination.x - transform.position.x) < .5f && Mathf.Abs(Destination.y - transform.position.y) < .5f)
        {
            _rigidbody.MovePosition(Destination);
            Plugged();
        }
    }

    public void UnPlugg()
    {
        pluggedIn = false;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    void Plugged()
    {
        pluggedIn = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    void Held()
    {
        _rigidbody.rotation = 0;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        pluggedIn = false;
        _rigidbody.gravityScale = 1;
    }
}
