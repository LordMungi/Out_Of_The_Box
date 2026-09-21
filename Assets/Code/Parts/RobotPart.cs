using UnityEngine;

public class RobotPart : MonoBehaviour, IGrabbable
{
    private Rigidbody _body;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void BeGrabbed()
    {
        _body.isKinematic = true;
        _body.useGravity = false;
    }

    public void BeReleased()
    {
        _body.isKinematic = false;
        _body.useGravity = true;
        _body.linearVelocity = Vector3.zero;
    }
}
