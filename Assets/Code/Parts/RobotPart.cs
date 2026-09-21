using UnityEngine;

public class RobotPart : MonoBehaviour
{
    private Rigidbody _body;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void Grab()
    {
        _body.isKinematic = true;
        _body.useGravity = false;
    }

    public void Release()
    {
        _body.isKinematic = false;
        _body.useGravity = true;
        _body.linearVelocity = Vector3.zero;
    }

    public void Equip()
    {
        _body.isKinematic = true;
        _body.useGravity = false;
    }
}
