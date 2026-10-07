using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void Grab()
    {

    }

    public void Release()
    {

    }

    public void Move(Vector2 delta)
    {
        _body.MovePosition(_body.position + new Vector3(delta.x, 0, delta.y));
    }
}