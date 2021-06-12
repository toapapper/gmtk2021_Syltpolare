using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pickup funktion i kabelkontaktskriptet, flytta med force.
/// </summary>

public class CableScript : MonoBehaviour
{
    HingeJoint2D joint;
    SpriteRenderer sr;

    Color subtractColor = Color.cyan;

    float breakForce;

    void Start()
    {
        joint = GetComponent<HingeJoint2D>();
        breakForce = joint.breakForce;
        sr = GetComponent<SpriteRenderer>();
        
    }
    
    private void OnJointBreak2D(Joint2D joint)
    {
        Debug.Log("Joint broken");
    }

    void Update()
    {
        if(joint != null)
            sr.color = Color.Lerp(Color.white, Color.red, joint.reactionForce.magnitude / breakForce);
    }
}
