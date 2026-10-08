using System;
using UnityEngine;

public class MoveHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    public bool IsGrounded => _controller.isGrounded;
    
    private CharacterController _controller;
    private Transform _mainCamera;

    private float _currentVelocityY;
    private bool _wasGrounded = false;
    private float _superJumpTimer;
    private bool _isChargingSuperJump = false;

    private const float _gravity = -9.81f;

    private Vector3 _currentHorizontalMove = Vector3.zero;

    #region Initialization

    public MoveHandler(CharacterController controller)
    {
        _controller = controller;
        _mainCamera = Camera.main.transform;
    }

    public override void Enable()
    {
        EventBus.Subscribe<OnJumpRequest>(OnJumpRequest);
        EventBus.Subscribe<OnSuperJumpRequest>(OnSuperJumpRequest);
        EventBus.Subscribe<OnGlideRequest>(OnGlideRequest);

        base.Enable();
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnJumpRequest>(OnJumpRequest);
        EventBus.Unsubscribe<OnSuperJumpRequest>(OnSuperJumpRequest);
        EventBus.Unsubscribe<OnGlideRequest>(OnGlideRequest);

        base.Disable();
    }

    #endregion

    #region Methods

    public void Update()
    {
        if (_isChargingSuperJump)
        {
            _superJumpTimer += Time.deltaTime;
            EventBus.Raise<OnSuperJumpChargeUpdated>(_superJumpTimer);
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        Vector3 camForward = _mainCamera.forward;
        Vector3 camRight = _mainCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction3D = (camForward * direction.y) + (camRight * direction.x);
        _currentHorizontalMove = direction3D * speed;
    }

    public void Fall(float fallSpeed)
    {
        if (_controller.isGrounded && _currentVelocityY < 0.0f)
        {
            _currentVelocityY = -2.0f;

            if (!_wasGrounded)
                EventBus.Raise<OnPlayerLanded>();
        }
        else
        {
            _currentVelocityY += _gravity * Time.deltaTime;
        }

        _wasGrounded = _controller.isGrounded;

        Vector3 finalMovement = _currentHorizontalMove;
        finalMovement.y = fallSpeed * _currentVelocityY;

        _controller.Move(finalMovement * Time.deltaTime);

        _currentHorizontalMove = Vector3.zero;
    }

    public void GlideFall(float maxGlideFallSpeed)
    {
        if (_currentVelocityY < -maxGlideFallSpeed)
        {
            _currentVelocityY = -maxGlideFallSpeed;
        }
        else
        {
            _currentVelocityY += _gravity * Time.deltaTime;
        }

        _wasGrounded = _controller.isGrounded;

        Vector3 finalMovement = _currentHorizontalMove;
        finalMovement.y = _currentVelocityY;

        _controller.Move(finalMovement * Time.deltaTime);
        _currentHorizontalMove = Vector3.zero;
    }
    
    public void Jump(float jumpForce)
    {
        _currentVelocityY = Mathf.Sqrt(jumpForce * -2f * _gravity);
        _controller.Move(1 * _currentVelocityY * Time.deltaTime * Vector3.up);
    }

    public void StartSuperJumpCharge()
    {
        _superJumpTimer = 0;
        _isChargingSuperJump = true;
    }

    public void PerformSuperJump(float superJumpMaxTime, float superJumpMaxForce, float superJumpMinForce)
    {
        float timePressed = Mathf.Min(superJumpMaxTime, _superJumpTimer);

        float superJumpForce = Mathf.Max(timePressed * superJumpMaxForce / superJumpMaxTime, superJumpMinForce);

        _currentVelocityY = Mathf.Sqrt(superJumpForce * -2f * _gravity);
        _controller.Move(1 * _currentVelocityY * Time.deltaTime * Vector3.up);

        _isChargingSuperJump = false;
        EventBus.Raise<OnSuperJumpChargeEnded>();
    }

    #endregion

    #region Callbacks

    private void OnJumpRequest(in OnJumpRequest context)
    {
        if (_controller.isGrounded)
        {
            EventBus.Raise<OnJumpRequestAccepted>();
        }
    }

    private void OnGlideRequest(in OnGlideRequest context)
    {
        if (!_controller.isGrounded && _currentVelocityY <= 0.0f)
        {
            EventBus.Raise<OnGlideRequestAccepted>();
        }
    }

    private void OnSuperJumpRequest(in OnSuperJumpRequest context)
    {
        EventBus.Raise<OnPartStateChangeAccepted>(BehaviourFSM.State.SuperJump);
    }

    #endregion
}