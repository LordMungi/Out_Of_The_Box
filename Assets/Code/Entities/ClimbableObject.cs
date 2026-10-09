using UnityEngine;

public class ClimbableObject : MonoBehaviour
{
    [SerializeField] private ClimbableObjectTopCollider _topArea;

    [field: SerializeField] public Transform ClimbOffset { get; private set; }
    [field: SerializeField] public Transform TopOffset { get; private set; }

    public bool IsPlayerOnTop { get { return _topArea.IsPlayerInside; } private set { } }

    private Plane _wallPlane;
    public Plane WallPlane { get { return _wallPlane; } private set { } }

    private float _offsetFromWall;
    public float OffsetFromWall { get { return _offsetFromWall; } private set { } }

    private float _offsetFromTop;
    public float OffsetFromTop { get { return _offsetFromTop; } private set { } }

    private void Awake()
    {
        Vector3 offsetFromFront = transform.position - ClimbOffset.position;
        offsetFromFront = new Vector3(offsetFromFront.x, 0, offsetFromFront.z);
        _offsetFromWall = offsetFromFront.magnitude;

        _wallPlane = new Plane(-offsetFromFront, transform.position);

        Debug.Log(TopOffset.position);
        Debug.Log(_wallPlane.ClosestPointOnPlane(TopOffset.position));
        _offsetFromTop = (TopOffset.position - _wallPlane.ClosestPointOnPlane(TopOffset.position)).magnitude;
        Debug.Log(_offsetFromTop);
    }
}