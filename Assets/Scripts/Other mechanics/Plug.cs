using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public Vector3 Destination;
    public bool held;
    public bool attracted;
    public bool pluggedIn;
    Rigidbody2D rigidbody;

    public GameObject preceedingCableSegment;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attracted == true && held == false)
        {
            Attracted();
        }
        if (held == true)
        {
            Held();
        }
    }

    void Attracted()
    {
        rigidbody.gravityScale = 0;
        rigidbody.MovePosition(Destination - transform.position * Time.deltaTime);
        if (Mathf.Abs(Destination.x - transform.position.x) < 0.05)
        {
            rigidbody.MovePosition(Destination);
            Plugged();
        }
    }

    void Plugged()
    {
        pluggedIn = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Held()
    {
        rigidbody.rotation = 0;
        rigidbody.constraints = RigidbodyConstraints2D.None;
        pluggedIn = false;
        rigidbody.gravityScale = 1;
    }
}