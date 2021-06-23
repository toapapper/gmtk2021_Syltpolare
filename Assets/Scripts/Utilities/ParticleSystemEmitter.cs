using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private void OnEnable()
    {
        _particleSystem.Play();
    }

    private void OnDisable()
    {
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
}
