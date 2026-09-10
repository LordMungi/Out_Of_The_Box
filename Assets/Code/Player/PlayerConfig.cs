using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Moving")]
    public MoveHandler.Data movingMoveData;
    [Header("Jumping")]
    public MoveHandler.Data jumpingMoveData;
    public JumpHandler.Data jumpingJumpData;
}
