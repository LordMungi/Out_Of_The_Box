using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Moving")]
    [SerializeField] public float moveSpeed;
    [Header("Jumping")]
    [SerializeField] public float jumpMoveSpeed;
    [SerializeField] public float jumpForce;
}
