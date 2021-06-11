using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Celezt.Times;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _horizontalForce = 4000;
    [SerializeField] private float _horizontalDrag = 0.1f;
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10000;
    [SerializeField] private float _jumpDrag = 0.1f;
    [SerializeField] private float _jumpTime = 0.5f;
    [SerializeField] private BoxCollider2D _groundCollider;
    [SerializeField] private LayerMask _groundCollisionMask;
    [Header("Fall")]
    [SerializeField] private float _fallDrag = 0.01f;

    private Rigidbody2D _rigidbody2D;

    private Duration _jumpDuration;

    private float _horizontalValue;
    private bool _isJumping;

    public void OnMoveHorizontal(float value)
    {
        _horizontalValue = value;
    }
    public void OnJump(float value)
    {
        switch (value)
        {
            case 1:
                if (Physics2D.OverlapBox(_groundCollider.transform.position, _groundCollider.size, 0, _groundCollisionMask))
                {
                    _jumpDuration = new Duration(_jumpTime);
                    _isJumping = true;
                }
                break;
            default:
                _isJumping = false;
                break;
        }
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 combinedForce = _rigidbody2D.velocity;

        combinedForce += new Vector2(_horizontalValue * _horizontalForce, 0);

        if (_isJumping && _jumpDuration.IsActive)
        {
            combinedForce += new Vector2(0, _jumpForce);
        }

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
