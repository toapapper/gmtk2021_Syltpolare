using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyStack : MonoBehaviour
{
    public int Count => _count;
    private float StackedForces => _stackedForces;

    private Rigidbody2D _rigidbody;
    private List<RigidbodyStack> _underObject = new List<RigidbodyStack>();
    private List<RigidbodyStack> _overObjects = new List<RigidbodyStack>();

    private int _count;
    private float _stackedForces;

    public (int, float) GetRecursiveUpData()
    {
        int count = 0;
        float stackedForces = 0;

        for (int i = 0; i < _overObjects.Count; i++)
        {
            (int, float) data = _overObjects[i].GetRecursiveUpData();
            count += data.Item1;
            stackedForces += data.Item2;
        }

        _count = count;
        _stackedForces = stackedForces;

        count += 1;
        stackedForces += Mathf.Abs(_rigidbody.mass * Physics.gravity.y);

        return (count, stackedForces);
    }

    public void GetRecursiveDownData(int count, float stackedForces, bool init)
    {
        if (!init)
        {
            _count = count;
            _stackedForces = stackedForces;

            count += 1;
            stackedForces += Mathf.Abs(_rigidbody.mass * Physics.gravity.y);
        }

        for (int i = 0; i < _underObject.Count; i++)
        {
            _underObject[i].GetRecursiveDownData(count, stackedForces, false);
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == tag)    // If same tag, meaning another robot.
        {
            Vector2 direction = collision.GetContact(0).normal;
            if (direction == Vector2.down)
            {
                RigidbodyStack rigidbodyStack = obj.GetComponent<RigidbodyStack>();

                _overObjects.Add(rigidbodyStack);
                rigidbodyStack._underObject.Add(this);
                (int, float) data = GetRecursiveUpData();
                GetRecursiveDownData(data.Item1, data.Item2, true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == tag)    // If same tag, meaning another robot.
        {
            RigidbodyStack rigidbodyStack = obj.GetComponent<RigidbodyStack>();
            if (_overObjects.Contains(rigidbodyStack))
            {
                _overObjects.Remove(rigidbodyStack);
                rigidbodyStack._underObject.Remove(this);
                (int, float) data = GetRecursiveUpData();
                GetRecursiveDownData(data.Item1, data.Item2, true);
            }
        }
    }
}
