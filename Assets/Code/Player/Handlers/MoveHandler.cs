using System;
using UnityEngine;

public class MoveHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    public bool IsGrounded => _controller.isGrounded;
    
    public Vector3 Position
    {
        get
        {
            return _controller.transform.position;
        }
        private set { }
    }

    private CharacterController _controller;
    private Transform _rootBone;
    private Transform _mainCamera;

    private Vector3 _velocity;

    private bool _wasGrounded = false;
    private float _superJumpTimer;
    private bool _isChargingSuperJump = false;

    private bool _hasTeleportedThisFrame = false;

    #region Initialization

    public MoveHandler(CharacterController controller, Transform rootBone)
    {
        _controller = controller;
        _rootBone = rootBone;
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

        if (!_hasTeleportedThisFrame)
            _controller.Move(_velocity * Time.deltaTime);
        _hasTeleportedThisFrame = false;

    }

    public void Move(Vector2 direction, float acceleration, float maxSpeed, float bodyRotationSpeed)
    {
        Vector3 camForward = _mainCamera.forward;
        Vector3 camRight = _mainCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 direction3D = ((camForward * direction.y) + (camRight * direction.x)).normalized * acceleration * Time.deltaTime;

        _velocity.x += direction3D.x;
        _velocity.z += direction3D.z;

        Vector3 clampedVelocity = Vector3.ClampMagnitude(new Vector3(_velocity.x, 0, _velocity.z), maxSpeed);
        _velocity.x = clampedVelocity.x;
        _velocity.z = clampedVelocity.z;

        Quaternion baseTargetRotation = Quaternion.LookRotation(direction3D.normalized);

        _rootBone.rotation = Quaternion.Slerp
        (
            _rootBone.rotation,
            baseTargetRotation,
            bodyRotationSpeed * Time.deltaTime
        );
    }

    public void MoveLinear(Vector2 delta)
    {
        MoveLinear(new Vector3(delta.x, 0, delta.y));
    }
    public void MoveLinear(Vector3 delta)
    {
        if (_hasTeleportedThisFrame)
            return;

        _controller.Move(delta);
    }

    public void SetPosition(Vector3 position)
    {
        _controller.enabled = false;
        _controller.transform.position = position;
        _controller.enabled = true;
        _velocity = Vector3.zero;

        _hasTeleportedThisFrame = true;
    }

    public void SetRotation(Quaternion rotation)
    {
        _rootBone.transform.rotation = rotation;
        Debug.Log("Set rotation: " + rotation);
    }

    public void UpdateFriction(float decelerationSpeed)
    {
        Vector3 newVelocity = new Vector3(_velocity.x, 0, _velocity.z);

        newVelocity = Vector3.MoveTowards(newVelocity, Vector3.zero, decelerationSpeed * Time.deltaTime);

        _velocity.x = newVelocity.x;
        _velocity.z = newVelocity.z;
    }

    public void UpdateGravity(float fallSpeed)
    {
        if (_controller.isGrounded && _velocity.y < 0.0f)
        {
            _velocity.y = -2.0f;

            if (!_wasGrounded)
                EventBus.Raise<OnPlayerLanded>();
        }
        else
        {
            _velocity.y -= fallSpeed * Time.deltaTime;
        }
        _wasGrounded = _controller.isGrounded;
    }

    public void GlideFall(float maxGlideFallSpeed)
    {
        if (_hasTeleportedThisFrame)
            return;

        if (_controller.isGrounded && _velocity.y < 0.0f)
        {
            _velocity.y = -2.0f;

            if (!_wasGrounded)
                EventBus.Raise<OnPlayerLanded>();
        }
        else if (_velocity.y < -maxGlideFallSpeed)
        {
            _velocity.y = -maxGlideFallSpeed;
        }
        else
        {
            _velocity.y -= Time.deltaTime;
        }
        _wasGrounded = _controller.isGrounded;
    }

    public void Jump(float jumpForce)
    {
        _velocity.y = Mathf.Sqrt(jumpForce * -2f * -9.8f);
        _controller.Move(1 * _velocity.y * Time.deltaTime * Vector3.up);
    }

    public void StartSuperJumpCharge()
    {
        _superJumpTimer = 0;
        _isChargingSuperJump = true;
    }

    public void PerformSuperJump(float superJumpMaxTime, float superJumpMaxForce, float superJumpMinForce)
    {
        if (_hasTeleportedThisFrame)
            return;

        float timePressed = Mathf.Min(superJumpMaxTime, _superJumpTimer);

        float superJumpForce = Mathf.Max(timePressed * superJumpMaxForce / superJumpMaxTime, superJumpMinForce);

        Jump(superJumpForce);

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
        if (!_controller.isGrounded && _velocity.y <= 0.0f)
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