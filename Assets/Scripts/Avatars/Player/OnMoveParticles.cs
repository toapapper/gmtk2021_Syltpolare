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
                    ParticleSystem.EmissionModule emission = _particleSystem.emission;
                    emission.enabled = true;
                }
                break;
            case GroundCheck.TriggerState.Elevation:
                {
                    ParticleSystem.EmissionModule emission = _particleSystem.emission;
                    emission.enabled = false;
                }
                break;
        }
    }
}
