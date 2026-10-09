using UnityEngine;

public class ClimbableObject : MonoBehaviour
{
    [SerializeField] private ClimbableObjectTopCollider _topArea;

    [field: SerializeField] public Transform ClimbOffset { get; private set; }
    [field: SerializeField] public Transform TopOffset { get; private set; }

    public bool IsPlayerOnTop { get { return _topArea.IsPlayerInside; } private set { } }

}