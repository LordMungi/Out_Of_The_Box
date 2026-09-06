using UnityEngine;

public class MovementHandler : PlayerHandler
{
    public MovementHandler(PlayerController p) : base(p) { }

    public void Move(Vector2 direction)
    {
        _player.transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * 5;
    }
}
