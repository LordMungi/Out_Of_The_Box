using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{

    [field: SerializeField] public float maxMoveSpeed { get; private set; }
    [field: SerializeField] public float acceleration { get; private set; }
    [field: SerializeField] public float decceleration { get; private set; }
    [field: SerializeField] public float bodyRotationSpeed { get; private set; }
    [field: SerializeField] public float airborneAcceleration { get; private set; }
    [field: SerializeField] public bool airborneFriction { get; private set; }
    [field: SerializeField] public float jumpForce { get; private set; }
    [field: SerializeField] public float fallSpeed { get; private set; }
    [field: SerializeField] public float superJumpMinForce { get; private set; }
    [field: SerializeField] public float superJumpMaxForce { get; private set; }
    [field: SerializeField] public float superJumpMaxTime { get; private set; }
    [field: SerializeField] public float glidingSpeed { get; private set; }
    [field: SerializeField] public float pushSpeed { get; private set; }
    [field: SerializeField] public float bulletSpeed { get; private set; }
    [field: SerializeField] public float fireRate { get; private set; }
    [field: SerializeField] public float climbSpeed { get; private set; }
    [field: SerializeField] public float distanceToPushable { get; private set; }
}
