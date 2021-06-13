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


    void Start()
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
        if (pluggedIn == true && !Possess.Contains(transform.root.gameObject))
        {
            //Possess.Add(transform.root.gameObject);
        }
        else if (pluggedIn == false && Possess.Contains(transform.root.gameObject))
        {
            //Possess.Remove(transform.root.gameObject);
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

    public void UnPlugg()
    {
        pluggedIn = false;
        rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    void Plugged()
    {
        pluggedIn = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    void Held()
    {
        rigidbody.rotation = 0;
        rigidbody.constraints = RigidbodyConstraints2D.None;
        pluggedIn = false;
        rigidbody.gravityScale = 1;
    }
}
