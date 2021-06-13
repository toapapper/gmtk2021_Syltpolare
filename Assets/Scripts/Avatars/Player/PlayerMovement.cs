using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Celezt.Times;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(RigidbodyStack))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _horizontalForce = 4000;
    [SerializeField] private float _horizontalDrag = 0.1f;
    [Header("Jump")]
    [SerializeField] private float _jumpImpulse = 200;
    [SerializeField] private float _jumpDrag = 0.1f;
    [SerializeField] private float _jumpTime = 0.5f;
    [SerializeField] private int _bodiesOnTopLimit = 1;
    [SerializeField] private BoxCollider2D _groundCollider;
    [SerializeField] private LayerMask _groundCollisionMask;
    [Header("Fall")]
    [SerializeField] private float _fallDrag = 0.01f;

    private Rigidbody2D _rigidbody2D;
    private RigidbodyStack _rigidbodyStack;

    private Duration _jumpDuration;

    private float _horizontalValue;
    private bool _isJumping;

    public void OnMoveHorizontal(InputContext context)
    {
        if (_rigidbodyStack.Count <= _bodiesOnTopLimit)
            _horizontalValue = context.Value;
        else
            _horizontalValue = 0;
    }
    public void OnJump(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                if (_rigidbodyStack.Count <= _bodiesOnTopLimit)
                {
                    Collider2D[] colliders2D = Physics2D.OverlapBoxAll(_groundCollider.transform.position, _groundCollider.size, 0, _groundCollisionMask);

                    for (int i = 0; i < colliders2D.Length; i++)
                    {
                        if (colliders2D[i].gameObject != gameObject)
                        {
                            _jumpDuration = new Duration(_jumpTime);
                            _isJumping = true;
                        }
                    }
                }

                Possess.Remove(gameObject);
                break;
            default:
                _isJumping = false;
                break;
        }
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbodyStack = GetComponent<RigidbodyStack>();
    }

    private void FixedUpdate()
    {
        Vector2 combinedForce = Vector2.zero;
        Vector2 combinedImpulse = Vector2.zero;

        combinedForce += new Vector2(_horizontalValue * _horizontalForce, 0);

        if (_isJumping && _jumpDuration.IsActive)
        {
            combinedImpulse += new Vector2(0, _jumpImpulse);

        }

        _rigidbody2D.AddForce(combinedImpulse, ForceMode2D.Impulse);
        _rigidbody2D.AddForce(combinedForce);
        Drag();
    }

    private void Drag()
    {
        Vector2 velocity = _rigidbody2D.velocity;
        velocity.x *= 1.0f - _horizontalDrag;
        velocity.y *= (_isJumping) ? 1.0f - _jumpDrag : 1.0f - _fallDrag;
        _rigidbody2D.velocity = velocity;
    }
}
