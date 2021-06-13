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

    private LayerMask _slopeCollisionMask;
    [SerializeField] private float slopeUpwardsMultiplier = 2.5f;

    [Header("Fall")]
    [SerializeField] private float _fallDrag = 0.01f;
    
    
    private Rigidbody2D _rigidbody2D;
    private RigidbodyStack _rigidbodyStack;

    private Duration _jumpDuration;

    private float _horizontalValue;
    private bool _isJumping;
    [SerializeField] private bool _isFlipped;

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
        _slopeCollisionMask = LayerMask.GetMask("StaticObstacle");
    }

    private void FixedUpdate()
    {
        Vector2 combinedForce = Vector2.zero;
        Vector2 combinedImpulse = Vector2.zero;

        #region slopedetection
        bool standingOnSlope = false;
        float slopeAngle = 0;
        RaycastHit2D leftRC = Physics2D.Raycast(new Vector2(_groundCollider.bounds.min.x, _groundCollider.bounds.max.y), Vector2.down, _groundCollider.size.y, _slopeCollisionMask);
        RaycastHit2D rightRC = Physics2D.Raycast(new Vector2(_groundCollider.bounds.max.x, _groundCollider.bounds.max.y), Vector2.down, _groundCollider.size.y, _slopeCollisionMask);

        Debug.DrawLine(new Vector2(_groundCollider.bounds.min.x, _groundCollider.bounds.max.y), new Vector2(_groundCollider.bounds.min.x, _groundCollider.bounds.max.y) + Vector2.down * _groundCollider.size.y, Color.red);
        Debug.DrawLine(new Vector2(_groundCollider.bounds.max.x, _groundCollider.bounds.max.y), new Vector2(_groundCollider.bounds.max.x, _groundCollider.bounds.max.y) + Vector2.down * _groundCollider.size.y, Color.red);

        float leftAngle = 0;
        float rightAngle = 0;

        if (leftRC.collider != null)
            leftAngle = Vector2.SignedAngle(Vector2.up, leftRC.normal);

        if (rightRC.collider != null)
            rightAngle = Vector2.SignedAngle(Vector2.up, rightRC.normal);

        if(leftAngle != 0)
        {
            standingOnSlope = true;
            slopeAngle = leftAngle;
        }
        else if(rightAngle != 0)
        {
            standingOnSlope = true;
            slopeAngle = rightAngle;
        }
        #endregion


        if (!standingOnSlope)
        {
            combinedForce += new Vector2(_horizontalValue * _horizontalForce, 0);
        }
        else if (standingOnSlope)
        {
            Vector2 forceToUse = new Vector2();
            forceToUse.x = _horizontalValue * _horizontalForce * Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
            forceToUse.y = _horizontalValue * _horizontalForce * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
            if (forceToUse.y > 0)//går uppför
                forceToUse.y *= slopeUpwardsMultiplier;

            combinedForce += forceToUse;
            Debug.Log("StandingOnSloep" + forceToUse);
        }



        if (_isJumping && _jumpDuration.IsActive)
        {
            combinedImpulse += new Vector2(0, _jumpImpulse);
        }

        //_rigidbody2D.AddRelativeForce(combinedImpulse * (_isFlipped ? -1 : 1), ForceMode2D.Impulse);
        _rigidbody2D.AddRelativeForce(combinedImpulse , ForceMode2D.Impulse);
        _rigidbody2D.AddRelativeForce(combinedForce * (_isFlipped ? -1 : 1));
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
