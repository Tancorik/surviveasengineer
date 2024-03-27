using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;

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
        _animator.SetFloat("ForwardSpeed", inputDirect.z);
        _animator.SetFloat("FlankSpeed", inputDirect.x);
        _animator.SetBool("isMove", inputDirect.magnitude != 0);
        inputDirect.y = _verticalForce;
        _characterController.Move(inputDirect * Time.deltaTime);
    }

    private Vector3 GetInputDirect() => new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _animator.SetBool("isGround", true);
            if (_verticalForce < 0) _verticalForce = -2;
            if (Input.GetButton("Jump"))
            {
                _verticalForce = _jumpForce;
                _animator.SetTrigger("JumpTrigger");
                _animator.SetBool("isGround", false);
            }
        }

        _verticalForce += _gravityForce * Time.deltaTime;
    }

}
