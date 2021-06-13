using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSegmentScript : MonoBehaviour
{
    CableBaseScript cableBase;

    public CableSegmentScript preceedingSegment = null;
    public CableSegmentScript nextSegment = null;

    Animator animator;

    private float animationLength = .667f;
    private float animTimer = 0;

    private float killNextTime = .05f;
    private float killNextTimer = 0;

    public bool dieing = false;

    // Start is called before the first frame update
    void Start()
    {
        cableBase = transform.GetComponentInParent<CableBaseScript>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animTimer > 0)
        {
            animTimer -= Time.deltaTime;

            if (animTimer <= 0)
            {
                cableBase.CableDied();
                Destroy(gameObject, .01f);
            }
        }

        if(killNextTimer > 0)
        {
            killNextTimer -= Time.deltaTime;
            if(killNextTimer <= 0)
            {
                if (preceedingSegment != null && preceedingSegment.dieing != true)
                    preceedingSegment.Kill();
                if (nextSegment != null && nextSegment.dieing != true)
                    nextSegment.Kill();
            }
                
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

}
