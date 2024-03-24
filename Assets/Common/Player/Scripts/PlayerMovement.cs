using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 10f;
    private float _jumpForce = 10f;
    private float _gravityForce = -30f;

    private CharacterController _characterController;

    private float _verticalForce;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Jump();

        Vector3 inputDirect = GetInputDirect() * _moveSpeed;

        inputDirect.y = _verticalForce;
        _characterController.Move(inputDirect * Time.deltaTime);
    }

    private Vector3 GetInputDirect() => new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            if (_verticalForce < 0) _verticalForce = -2;
            if (Input.GetButton("Jump")) _verticalForce = _jumpForce;
        }

        _verticalForce += _gravityForce * Time.deltaTime;
    }

}
