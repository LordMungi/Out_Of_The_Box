using System;
using UnityEngine;

public class JumpHandler : PlayerHandler
{
    [Serializable] public struct Data
    {
        public float _jumpForce;
        public float _fallSpeed;
    }

    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private CharacterController _controller;
    private Data _data;

    private float _currentVelocity;

    public JumpHandler(CharacterController controller, Data data)
    {
        _controller = controller;
        _data = data;

    }
    public void Fall()
    {
        if (_controller.isGrounded)
            EventBus.Raise<OnPlayerLanded>();
        else
        {
            _currentVelocity -= _data._fallSpeed * Time.deltaTime;
            _controller.Move(new Vector3(0, _currentVelocity, 0));
        }
    }

    public void Jump()
    {
        _currentVelocity = _data._jumpForce;
        _controller.Move(new Vector3(0, _currentVelocity, 0));
    }
}
