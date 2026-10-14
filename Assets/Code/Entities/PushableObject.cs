using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody _body;
    private MeshFilter _mesh;

    private Vector3 _meshCenter;
    private Vector3 _meshSize;

    private LayerMask _minusPlayerLayer;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _mesh = GetComponent<MeshFilter>();
        _minusPlayerLayer = ~(1 << LayerMask.NameToLayer("Player"));

        _meshCenter = _mesh.sharedMesh.bounds.center;
        _meshSize = _mesh.sharedMesh.bounds.size;
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
        return !Physics.BoxCast(transform.TransformPoint(_meshCenter), Vector3.Scale(_meshSize, transform.lossyScale) / 2f, delta3.normalized, transform.rotation, delta3.magnitude, _minusPlayerLayer);
    }
}