using System;
using UnityEngine;

public class MoveHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [Serializable] public struct Data
    {
        public float speed;
        public float jumpForce;
        public float fallSpeed;
    }

    private CharacterController _controller;
    private Data _data;

    private float _currentVelocityY;
    private bool _wasGrounded = false;

    public MoveHandler(CharacterController controller, Data data)
    {
        _controller = controller;
        _data = data;
    }

    public void Move(Vector2 direction)
    {
        _controller.Move(new Vector3(direction.x, 0, direction.y) * _data.speed * Time.deltaTime);
    }

    public void Fall()
    {
        if (_controller.isGrounded)
        {
            _currentVelocityY = 0;

            if (!_wasGrounded)
                EventBus.Raise<OnPlayerLanded>();
        }
        else
        {
            _currentVelocityY -= _data.fallSpeed * Time.deltaTime;
        }
        _wasGrounded = _controller.isGrounded;

        _controller.Move(new Vector3(0, _currentVelocityY, 0));
    }

    public void Jump()
    {
        _currentVelocityY = _data.jumpForce;
        _controller.Move(new Vector3(0, _currentVelocityY, 0));
    }
}
