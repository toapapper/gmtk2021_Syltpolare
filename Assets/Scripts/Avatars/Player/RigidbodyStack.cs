using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyStack : MonoBehaviour
{
    public BodyStack Stack => _bodyStack;

    [SerializeField] private UnityEvent<BodyStack> _changedStackEvent;

    private Rigidbody2D _rigidbody;
    private List<RigidbodyStack> _underObject = new List<RigidbodyStack>();
    private List<RigidbodyStack> _overObjects = new List<RigidbodyStack>();

    private BodyStack _bodyStack;

    public struct BodyStack
    {
        public int StackedCount;
        public float StackedForce;
    }

    public BodyStack GetRecursiveUpData()
    {
        BodyStack bodyStack = new BodyStack { };

        for (int i = 0; i < _overObjects.Count; i++)
            bodyStack = _overObjects[i].GetRecursiveUpData();

        _bodyStack = bodyStack;

        bodyStack.StackedCount += 1;
        bodyStack.StackedForce += Mathf.Abs(_rigidbody.mass * Physics.gravity.y);

        _changedStackEvent.Invoke(_bodyStack);

        return bodyStack;
    }

    public void GetRecursiveDownData(BodyStack bodyStack, bool init)
    {
        if (!init)
        {
            _bodyStack = bodyStack;

            bodyStack.StackedCount += 1;
            bodyStack.StackedForce += Mathf.Abs(_rigidbody.mass * Physics.gravity.y);
        }

        for (int i = 0; i < _underObject.Count; i++)
            _underObject[i].GetRecursiveDownData(bodyStack, false);

        _changedStackEvent.Invoke(_bodyStack);
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
                BodyStack bodyStack = GetRecursiveUpData();
                GetRecursiveDownData(bodyStack, true);
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
                BodyStack bodyStack = GetRecursiveUpData();
                GetRecursiveDownData(bodyStack, true);
            }
        }
    }
}
