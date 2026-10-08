using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody _body;

    private LayerMask _minusPlayerLayer;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _minusPlayerLayer = ~(1 << LayerMask.NameToLayer("Player"));

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

    public bool CanMove(Vector2 delta)
    {
        Vector3 delta3 = new Vector3(delta.x, 0, delta.y);

        return !Physics.BoxCast(transform.position, transform.lossyScale / 2, delta3.normalized, transform.rotation, delta3.magnitude, _minusPlayerLayer);
    }
}