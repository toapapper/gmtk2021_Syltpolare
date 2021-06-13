using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _mainGameObject;

    private int _insideCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            _insideCollision++;

            if (_insideCollision >= 1)
            {
                ParticleSystem.EmissionModule emission = _particleSystem.emission;
                emission.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Mathf.Max(0, --_insideCollision);

            if (_insideCollision == 0)
            {
                ParticleSystem.EmissionModule emission = _particleSystem.emission;
                emission.enabled = false;
            }
        }
    }
}
