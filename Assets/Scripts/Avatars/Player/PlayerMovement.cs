using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Celezt.Times;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(RigidbodyStack))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _horizontalForceGrounded = 4000;
    [SerializeField] private float _horizontalForceElevation = 3000;
    [SerializeField] private float _horizontalDrag = 0.1f;
    [Header("Jump")]
    [SerializeField] private float _jumpImpulse = 200;
    [SerializeField] private float _jumpDrag = 0.1f;
    [SerializeField] private float _jumpTime = 0.5f;
    [SerializeField] private int _bodiesOnTopLimit = 1;
    //[SerializeField] private BoxCollider2D _groundCollider;
    [SerializeField] private LayerMask _groundMask;

    //private LayerMask _slopeCollisionMask;
    //[SerializeField] private float slopeUpwardsMultiplier = 2.5f;
    [Header("Slope")]
    [SerializeField] private float _slopeCheckDistance;

    [Header("Fall")]
    [SerializeField] private float _fallDrag = 0.01f;
    [SerializeField] private bool _isFlipped;

    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private RigidbodyStack _rigidbodyStack;

    private Vector2 _colliderSize;
    private Vector2 _slopeNormalPerpendicular;

    private Duration _jumpDuration;

    private float _slopeDownAngle;
    private float _slopeDownAngleOld;
    private float _horizontalValue;
    private bool _isOnSlope;
    private bool _isJumping;
    private bool _isGrounded;

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
                    if (_isGrounded)
                    {
                        _jumpDuration = new Duration(_jumpTime);
                        _isJumping = true;
                    }
                }

                break;
            default:
                _isJumping = false;
                break;
        }
    }

    public void OnGround(GroundCheck.TriggerState state, Collider2D collision)
    {
        switch (state)
        {
            case GroundCheck.TriggerState.Grounded:
                _isGrounded = true;
                break;
            case GroundCheck.TriggerState.Elevation:
                _isGrounded = false;
                break;
        }
    }

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbodyStack = GetComponent<RigidbodyStack>();
        //_slopeCollisionMask = LayerMask.GetMask("StaticObstacle");

        _colliderSize = _collider.size;
    }

    private void FixedUpdate()
    {
        Vector2 combinedForce = Vector2.zero;
        Vector2 combinedImpulse = Vector2.zero;

        Vector2 position = transform.position;

        //        #region Slope Detection
        //        bool standingOnSlope = false;
        //        float slopeAngle = 0;
        //        RaycastHit2D leftRC = Physics2D.Raycast(new Vector2(_groundCollider.bounds.min.x, _groundCollider.bounds.max.y), Vector2.down, _groundCollider.size.y, _slopeCollisionMask);
        //        RaycastHit2D rightRC = Physics2D.Raycast(new Vector2(_groundCollider.bounds.max.x, _groundCollider.bounds.max.y), Vector2.down, _groundCollider.size.y, _slopeCollisionMask);
        //#if UNITY_EDITOR
        //        Debug.DrawLine(new Vector2(_groundCollider.bounds.min.x, _groundCollider.bounds.max.y), new Vector2(_groundCollider.bounds.min.x, _groundCollider.bounds.max.y) + Vector2.down * _groundCollider.size.y, Color.red);
        //        Debug.DrawLine(new Vector2(_groundCollider.bounds.max.x, _groundCollider.bounds.max.y), new Vector2(_groundCollider.bounds.max.x, _groundCollider.bounds.max.y) + Vector2.down * _groundCollider.size.y, Color.red);
        //#endif
        //        float leftAngle = 0;
        //        float rightAngle = 0;

        //        if (leftRC.collider != null)
        //            leftAngle = Vector2.SignedAngle(Vector2.up, leftRC.normal);

        //        if (rightRC.collider != null)
        //            rightAngle = Vector2.SignedAngle(Vector2.up, rightRC.normal);

        //        if(leftAngle != 0)
        //        {
        //            standingOnSlope = true;
        //            slopeAngle = leftAngle;
        //        }
        //        else if(rightAngle != 0)
        //        {
        //            standingOnSlope = true;
        //            slopeAngle = rightAngle;
        //        }
        //        #endregion


        //if (!standingOnSlope)
        //{
        //    combinedForce += new Vector2(_horizontalValue * _horizontalForce, 0);
        //}
        //else if (standingOnSlope)
        //{
        //    Vector2 forceToUse = new Vector2();
        //    forceToUse.x = _horizontalValue * _horizontalForce * Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
        //    forceToUse.y = _horizontalValue * _horizontalForce * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
        //    if (forceToUse.y > 0)//går uppför
        //        forceToUse.y *= slopeUpwardsMultiplier;

        //    combinedForce += forceToUse;
        //}

        SlopeCheck(position);

        if (_isGrounded && !_isOnSlope)
        {
            combinedForce += new Vector2(_horizontalValue * _horizontalForceGrounded, 0);
        }
        else if (_isGrounded && _isOnSlope)
        {
            combinedForce += new Vector2(-_horizontalValue * _horizontalForceGrounded * _slopeNormalPerpendicular.x, -_horizontalValue * _horizontalForceGrounded * _slopeNormalPerpendicular.y);
        }
        else if (!_isGrounded)
        {
            combinedForce += new Vector2(_horizontalValue * _horizontalForceElevation, 0);
        }



        if (_isJumping && _jumpDuration.IsActive)
        {
            combinedImpulse += new Vector2(0, _jumpImpulse);
        }

        _rigidbody.AddRelativeForce(combinedImpulse , ForceMode2D.Impulse);
        _rigidbody.AddRelativeForce(combinedForce * (_isFlipped ? -1 : 1));
        ApplyDrag();
    }

    private void ApplyDrag()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x *= 1.0f - _horizontalDrag;
        velocity.y *= (_isJumping) ? 1.0f - _jumpDrag : 1.0f - _fallDrag;
        _rigidbody.velocity = velocity;
    }

    private void SlopeCheck(Vector2 position)
    {
        //Vector2 checkPosition = position - new Vector2(0.0f, _colliderSize.y / 2);
        Vector2 checkPosition = ((_horizontalValue >= 0) ?
            position + new Vector2(_colliderSize.x / 2, 0) :
            position - new Vector2(_colliderSize.x / 2, 0))
            - new Vector2(0.0f, _colliderSize.y / 2);

        SlopeCheckVertical(checkPosition);
    }

    private void SlopeCheckHorizontal(Vector2 checkPosition)
    {

    }

    private void SlopeCheckVertical(Vector2 checkPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, _slopeCheckDistance, _groundMask);

        if (hit)
        {
            _slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;

            _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (_slopeDownAngle != _slopeDownAngleOld)
                _isOnSlope = true;

            _slopeDownAngleOld = _slopeDownAngle;

            Debug.DrawRay(hit.point, _slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }
}
