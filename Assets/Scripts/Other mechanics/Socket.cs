using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public bool occupiced = false;
    public Collider2D occupiedBy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (occupiced == false)
        {
            if (collision.gameObject.tag == "Plug" && occupiedBy != collision && occupiedBy == null)
            {
                collision.gameObject.GetComponent<Plug>().Destination = transform.position;
                collision.gameObject.GetComponent<Plug>().attracted = true;
                occupiedBy = collision;
                occupiced = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (occupiced == true)
        {
            if (collision.gameObject.tag == "Plug" && collision == occupiedBy)
            {
                collision.gameObject.GetComponent<Plug>().attracted = false;
                occupiced = false;
            }
        }
        if (occupiced == false)
        {
            occupiedBy = null;
        }
    }

}
