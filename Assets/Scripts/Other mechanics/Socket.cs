using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public bool occupiced = false;
    public Collider2D occupiedBy;

    private void Start()
    {
        StartCoroutine(onCoroutine());
    }
    IEnumerator onCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (occupiedBy == null)
            {
                occupiced = false;
            }
            if (occupiced == false)
            {
                occupiedBy = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (occupiced == false)
        {
            if (collision.gameObject.tag == "Plug")
            {
                collision.gameObject.GetComponent<Plug>().Destination = transform.position;
                collision.gameObject.GetComponent<Plug>().attracted = true;
                occupiedBy = collision;
                occupiced = true;
            }
        }
        else if (occupiced == true)
        {
            if (collision == occupiedBy)
            {

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
        if (occupiedBy == null)
        {
            occupiced = false;
        }
    }

}
