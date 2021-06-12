using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyStack : MonoBehaviour
{
    public int BodiesOnTop => _bodiesOnTop;
    private float ForceOnTop => _forcesOnTop;

    private Rigidbody2D _rigidbody;
    private List<RigidbodyStack> _underObject = new List<RigidbodyStack>();
    private List<RigidbodyStack> _overObjects = new List<RigidbodyStack>();

    private int _bodiesOnTop;
    private float _forcesOnTop;

    public (int, float) GetRecursiveUpData()
    {
        int bodiesOnTop = 0;
        float forcesOnTop = 0;

        for (int i = 0; i < _overObjects.Count; i++)
        {
            (int, float) data = _overObjects[i].GetRecursiveUpData();
            bodiesOnTop += data.Item1;
            forcesOnTop += data.Item2;
        }

        _bodiesOnTop = bodiesOnTop;
        _forcesOnTop = forcesOnTop;

        bodiesOnTop += 1;
        forcesOnTop += Mathf.Abs(_rigidbody.mass * Physics.gravity.y);

        return (bodiesOnTop, forcesOnTop);
    }

    public void GetRecursiveDownData(int bodiesOnTop, float forcesOnTop, bool init)
    {
        if (!init)
        {
            _bodiesOnTop = bodiesOnTop;
            _forcesOnTop = forcesOnTop;

            bodiesOnTop += 1;
            forcesOnTop += Mathf.Abs(_rigidbody.mass * Physics.gravity.y);
        }

        for (int i = 0; i < _underObject.Count; i++)
        {
            _underObject[i].GetRecursiveDownData(bodiesOnTop, forcesOnTop, false);
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
