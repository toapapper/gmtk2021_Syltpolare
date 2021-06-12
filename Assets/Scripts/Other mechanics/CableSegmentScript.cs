using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSegmentScript : MonoBehaviour
{
    CableBaseScript cableBase;

    // Start is called before the first frame update
    void Start()
    {
        cableBase = transform.GetComponentInParent<CableBaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Dangerous"))
        {
            cableBase.OnCableBreak();
        }
    }

}