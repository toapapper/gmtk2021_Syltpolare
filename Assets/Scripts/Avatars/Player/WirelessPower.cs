using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirelessPower : MonoBehaviour
{
    [SerializeField]private bool onByDefault = true;
    private bool initialized = false;

    public bool powered { get; protected set; } = false;
    

    public void Update()
    {
        if (!initialized)
        {
            if (onByDefault)
                PowerOn();
            initialized = true;
        }
    }

    public void PowerOn()
    {
        if (powered)//redan på
            return;

        powered = true;
        if (!Possess.Contains(gameObject))
            Possess.Add(gameObject);
    }

    public void PowerOff()
    {
        if (!powered)
            return;

        powered = false;
        if (Possess.Contains(gameObject))
            Possess.Remove(gameObject);
    }
}
