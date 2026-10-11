using UnityEngine;

public struct PlayerData
{
    public PlayerController player;
    public CharacterController controller;
    public PlayerInputActions input;
    public PlayerConfig config;
    public Transform rootBone;
    public PlayerParents parents;
    public PlayerHandlers handlers;
}