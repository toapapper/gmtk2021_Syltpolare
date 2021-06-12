using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactKillScript : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Robot")
        {
            Destroy(collision.gameObject);
        }
    }
}
