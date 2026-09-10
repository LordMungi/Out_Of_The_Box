using System;
using UnityEngine;

public class MoveHandler : PlayerHandler
{
    [Serializable] public struct Data
    {
        public float speed;
    }

    private CharacterController _controller;
    private Data _data;

    public MoveHandler(CharacterController controller, Data data)
    {
        _controller = controller;
        _data = data;
    }

    public void Move(Vector2 direction)
    {
        _controller.Move(new Vector3(direction.x, 0, direction.y) * _data.speed * Time.deltaTime);
    }
}
