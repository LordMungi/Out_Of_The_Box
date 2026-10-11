using System;
using UnityEngine;

[Serializable] public struct PlayerData
{
    public PlayerController player;
    public CharacterController controller;
    public PlayerInputActions input;
    public PlayerConfig config;
    public Animator animator;
    public Transform rootBone;
    public Camera[] cameras;
    public PlayerParents parents;
    public PlayerHandlers handlers;
}