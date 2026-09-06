using UnityEngine;

public class MoveHandler : PlayerHandler
{
    private float _speed;

    public MoveHandler(ref PlayerData p, float speed) : base(ref p) 
    {
        _speed = speed;
    }

    public void Move(Vector2 direction)
    {
        _player.body.MovePosition(Vector3.MoveTowards(_player.body.position, _player.body.position + new Vector3(direction.x, 0, direction.y), Time.deltaTime * _speed));
    }
}
