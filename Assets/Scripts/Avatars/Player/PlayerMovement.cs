using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Times;
using MyBox;

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
    [SerializeField] private float _massLimit = 159.0f;

    [Header("Slope")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField, Min(0)] private int _leftSlopeIndexPoint;
    [SerializeField, Min(0)] private int _rightSlopeIndexPoint;
    [SerializeField, Min(0)] private int _leftLedgeIndexPoint;
    [SerializeField, Min(0)] private int _rightLedgeIndexPoint;
    [SerializeField] private float _slopeCheckDistance = 0.5f;
    [SerializeField] private float _ledgeCheckDistance = 0.5f;
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private Vector2 _ledgeForce = new Vector2(3000, 3000);
    [SerializeField] private bool _UseModifiedPhysicsMaterial;
    [SerializeField, ConditionalField(nameof(_UseModifiedPhysicsMaterial))] private PhysicsMaterial2D _defaultFricition;
    [SerializeField, ConditionalField(nameof(_UseModifiedPhysicsMaterial))] private PhysicsMaterial2D _slopeFriction;

    [Header("Fall")]
    [SerializeField] private float _fallDrag = 0.01f;
    [SerializeField] private bool _isFlipped;

    private Rigidbody2D _rigidbody;
    private RigidbodyStack _rigidbodyStack;
    private PolygonCollider2D _polyCollider;

    private Duration _jumpDuration;

    private Vector2 _slopeNormalPerpendicular;
    private Vector2 _point1;
    private Vector2 _point2;
    private Vector2 _point3;
    private Vector2 _point4;

    private float _slopeDownAngle;
    private float _slopeSideAngle;
    private float _slopeDownAngleOld;
    private float _horizontalValue;
    private bool _isLookingLeft;
    private bool _isOnSlope;
    private bool _isOnLedge;
    private bool _isSlopeHit;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _canWalkOnSlope;

    public void OnMoveHorizontal(InputContext context)
    {
        if (_rigidbodyStack != null && _rigidbodyStack.Stack.StackedMass <= _massLimit)
            _horizontalValue = context.Value;
        else
            _horizontalValue = 0;
    }
    public void OnJump(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                if (_rigidbodyStack.Stack.StackedMass <= _massLimit)
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

    void Awake()
    {
        _polyCollider = GetComponent<PolygonCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbodyStack = GetComponent<RigidbodyStack>();

        Vector2[] points = _polyCollider.points;
        _point1 = points[_leftSlopeIndexPoint];
        _point2 = points[_rightSlopeIndexPoint];
        _point3 = points[_leftLedgeIndexPoint];
        _point4 = points[_rightLedgeIndexPoint];
    }

    private void FixedUpdate()
    {
        Vector2 combinedForce = Vector2.zero;
        Vector2 combinedImpulse = Vector2.zero;

        Vector3 position = transform.position;
        Vector2 up = transform.up;
        Vector2 right = transform.right;

        if (_horizontalValue < 0)
            _isLookingLeft = false;
        else if (_horizontalValue > 0)
            _isLookingLeft = true;

        SlopeCheck(position, up, right);
        EdgeCheck(position, up);

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

    private void SlopeCheck(Vector3 position, Vector2 up, Vector2 right)
    {
        // Takes movement direction into account and checks in front of the collider.
        Vector2 checkPosition = (_isLookingLeft ?
            position - new Vector3(_point1.x, -_point1.y) :
            position - new Vector3(_point2.x, -_point2.y));

        SlopeCheckHorizontal(checkPosition, right);
        SlopeCheckVertical(checkPosition, up);
    }

    private void EdgeCheck(Vector3 position, Vector2 up)
    {
        // Takes movement direction into account and checks in front of the collider.
        Vector2 checkPosition = (_isLookingLeft ?
            position - new Vector3(_point3.x, -_point3.y) :
            position - new Vector3(_point4.x, -_point4.y));

        EdgeCheckVertical(position, checkPosition, up);
    }

    private void SlopeCheckHorizontal(Vector2 checkPosition, Vector2 right)
    {
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

    private void SlopeCheckVertical(Vector2 checkPosition, Vector2 up)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, -up, _slopeCheckDistance, _groundMask);

        if (hit && hit.transform.gameObject != gameObject)
        {
            _isSlopeHit = true;

            _slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;

            _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSlope = true;
            }

            _slopeDownAngleOld = _slopeDownAngle;

#if UNITY_EDITOR
            Debug.DrawRay(hit.point, _slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
#endif
        }
        else
            _isSlopeHit = false;

        _canWalkOnSlope = _slopeDownAngle <= _maxSlopeAngle;

        if (_UseModifiedPhysicsMaterial)
        {
            if (_isOnSlope && _horizontalValue == 0.0f && _canWalkOnSlope)
                _rigidbody.sharedMaterial = _slopeFriction;
            else
                _rigidbody.sharedMaterial = _defaultFricition;
        }
    }

    private void EdgeCheckVertical(Vector3 position, Vector2 checkPosition, Vector2 up)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, -up, _ledgeCheckDistance, _groundMask);

        if (hit && hit.transform.gameObject != gameObject)
        {
            _isOnLedge = !_isSlopeHit;

            if (_isOnLedge && _horizontalValue != 0 && !_isJumping) // Push if on a ledge.
                _rigidbody.AddForce(new Vector2((_isLookingLeft ? _ledgeForce.x : -_ledgeForce.x), _ledgeForce.y));

#if UNITY_EDITOR
            if (_isOnLedge)
                Debug.DrawRay(hit.point, hit.normal, Color.blue);
            else
                Debug.DrawRay(hit.point, hit.normal, Color.yellow);
#endif
        }
    }
}
