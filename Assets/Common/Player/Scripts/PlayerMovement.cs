using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private const float RotationSmoothTime = 0.12f;
    private const float WALK_MULTIPLIER = 1.0f;
    private const float RUN_MULTIPLER = 4.0f;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _cameraTarget;

    private CharacterController _characterController;

    private float _walkSpeed = 1.5f;
    private float _speedMuliplier = 1f;
    private float _jumpForce = 10f;
    private float _gravityForce = -30f;
    private float _topBorderSpeedMultiplier = RUN_MULTIPLER;

    private float _verticalForce = 0;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    
    private int _animMoveSpeedId;
    private int _animIsGroundId;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _animMoveSpeedId = Animator.StringToHash("MoveSpeed");
        _animIsGroundId = Animator.StringToHash("isGround");
    }

    private void Update()
    {
        SetSpeedMultiplier();
        Jump();
        Move();
    }

    private void Move()
    {
        Vector3 inputDirection = GetInputDirection();
        float inputMagnitude = inputDirection.magnitude;

        if (inputMagnitude > 0)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cameraTarget.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        Vector3 verticalVector = new Vector3(0.0f, _verticalForce, 0.0f) * Time.deltaTime;
        float speedMultiplier = _walkSpeed * _speedMuliplier * inputMagnitude * Time.deltaTime;
        _characterController.Move(targetDirection.normalized * speedMultiplier + verticalVector);

        _playerAnimator.SetFloat(_animMoveSpeedId, inputMagnitude * _speedMuliplier);
    }

    private Vector3 GetInputDirection() => new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _playerAnimator.SetBool(_animIsGroundId, true);
            if (_verticalForce < 0) _verticalForce = -2;
            if (Input.GetButton("Jump")) _verticalForce = _jumpForce;
        } else
        {
            _playerAnimator.SetBool(_animIsGroundId, false);
        }

        _verticalForce += _gravityForce * Time.deltaTime;
    }

    private void SetSpeedMultiplier()
    {
        float sign = Input.GetKey(KeyCode.LeftShift) ? 1 : -1;
        _speedMuliplier += Time.deltaTime * 5 * sign;
        _speedMuliplier = Mathf.Clamp(_speedMuliplier, WALK_MULTIPLIER, _topBorderSpeedMultiplier);
    }
}
