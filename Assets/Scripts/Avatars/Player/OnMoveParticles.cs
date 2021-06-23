using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMoveParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void OnGroundChange(GroundCheck.TriggerState state, Collider2D collision)
    {
        switch (state)
        {
            case GroundCheck.TriggerState.Grounded:
                {
                    _particleSystem.Play();
                }
                break;
            case GroundCheck.TriggerState.Elevation:
                {
                    _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
                break;
        }
    }
}
