using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Celezt.Times;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(RigidbodyStack))]
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

    [Header("Slope")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _slopeCheckDistance = 0.5f;
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private PhysicsMaterial2D _defaultFricition;
    [SerializeField] private PhysicsMaterial2D _slopeFriction;

    [Header("Fall")]
    [SerializeField] private float _fallDrag = 0.01f;
    [SerializeField] private bool _isFlipped;

    private Rigidbody2D _rigidbody;
    private RigidbodyStack _rigidbodyStack;
    private PolygonCollider2D _polyCollider;

    private Vector2 _slopeNormalPerpendicular;
    private Vector2 _point1;
    private Vector2 _point2;

    private Duration _jumpDuration;

    private float _slopeDownAngle;
    private float _slopeSideAngle;
    private float _slopeDownAngleOld;
    private float _horizontalValue;
    private bool _isOnSlope;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _canWalkOnSlope;

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
                    if (_isGrounded && _canWalkOnSlope)
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
        _polyCollider = GetComponent<PolygonCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbodyStack = GetComponent<RigidbodyStack>();

        Vector2[] points = _polyCollider.points;
        _point1 = points[5];
        _point2 = points[6];
    }

    private void FixedUpdate()
    {
        Vector2 combinedForce = Vector2.zero;
        Vector2 combinedImpulse = Vector2.zero;

        Vector2 position = transform.position;

        SlopeCheck(position);

        if (_isGrounded && !_isOnSlope)
        {
            combinedForce += new Vector2(_horizontalValue * _horizontalForceGrounded, 0);
        }
        else if (_isGrounded && _isOnSlope && _canWalkOnSlope)
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
        // Takes movement direction into account and checks in front of the collider.
        Vector2 checkPosition = ((_horizontalValue >= 0) ?
            position - new Vector2(_point1.x, -_point1.y) :
            position - new Vector2(_point2.x, -_point2.y));

        SlopeCheckHorizontal(checkPosition);
        SlopeCheckVertical(checkPosition);
    }

    private void SlopeCheckHorizontal(Vector2 checkPosition)
    {
        Vector2 right = transform.right;
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPosition, right, _slopeCheckDistance, _groundMask);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPosition, -right, _slopeCheckDistance, _groundMask);

        if (slopeHitFront)
        {
            _isOnSlope = true;
            _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            _isOnSlope = true;
            _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            _isOnSlope = false;
            _slopeSideAngle = 0.0f;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, -transform.up, _slopeCheckDistance, _groundMask);

        if (hit)
        {
            _slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;

            _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSlope = true;
            }

            _slopeDownAngleOld = _slopeDownAngle;

            Debug.DrawRay(hit.point, _slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        _canWalkOnSlope = _slopeDownAngle <= _maxSlopeAngle;

        // Change friction.
        if (_isOnSlope && _horizontalValue == 0.0f && _canWalkOnSlope)
            _rigidbody.sharedMaterial = _slopeFriction;
        else
            _rigidbody.sharedMaterial = _defaultFricition;
    }
}
