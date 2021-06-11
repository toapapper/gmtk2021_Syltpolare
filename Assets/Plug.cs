using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public Vector2 Destination;
    bool held;
    public bool attracted;
    bool pluggedIn;
    Rigidbody2D rigidbody;

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
    }

    void Attracted()
    {

        transform.position = Vector2.Lerp(transform.position, Destination, Time.deltaTime * 1.5f);

        if (Mathf.Abs(Destination.x - transform.position.x) < 0.05)
        {
            transform.position = Destination;
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
        rigidbody.constraints = RigidbodyConstraints2D.None;
        pluggedIn = false;
    }
}
