using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float moveSpeed;
    [field: SerializeField] public float airborneSpeed;
    [field: SerializeField] public float jumpForce;
    [field: SerializeField] public float fallSpeed;
    [field: SerializeField] public float superJumpMinForce;
    [field: SerializeField] public float superJumpMaxForce;
    [field: SerializeField] public float superJumpMaxTime;
    [field: SerializeField] public float pushSpeed;

}
