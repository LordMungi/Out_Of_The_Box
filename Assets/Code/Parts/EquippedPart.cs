using UnityEngine;

public class EquippedPart : MonoBehaviour
{
    [field: SerializeField] public RobotPart.Type type { get; private set; }
}