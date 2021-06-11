using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _horizontalForce = 2000;

    private Rigidbody2D _rigidbody2D;

    private float _horizontalValue;

    public void MoveHorizontal(float value)
    {
        _horizontalValue = value;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(new Vector2(_horizontalValue * _horizontalForce, 0));
    }
}
