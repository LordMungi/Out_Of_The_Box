using System;
using UnityEngine;

public class MoveHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [Serializable] public struct Data
    {
        public float speed;
        public float jumpForce;
    }

    private CharacterController _controller;
    private Data _data;

    private float _currentVelocityY;
    private bool _wasGrounded = false;

    private const float _gravity = -9.81f;
    
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
            _currentVelocityY = 0f;
            
            if (!_wasGrounded)
                EventBus.Raise<OnPlayerLanded>();
        }
        else
        {
            _currentVelocityY += _gravity * Time.deltaTime;
        }
        _wasGrounded = _controller.isGrounded;

        _controller.Move(new Vector3(0, _data.speed * _currentVelocityY * Time.deltaTime, 0));
    }

    public void Jump()
    {
        _currentVelocityY = Mathf.Sqrt(_data.jumpForce * -2f * _gravity);
        _controller.Move(_data.speed * _currentVelocityY * Time.deltaTime * Vector3.up);
    }
}
