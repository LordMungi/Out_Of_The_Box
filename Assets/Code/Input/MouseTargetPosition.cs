using Unity.VisualScripting;
using UnityEngine;

public class MouseTargetPosition
{
    private PlayerController.BoneConfig[] _bodyBones;

    private float _headRotationSpeed = 25.0f;
    private float _bodyRotationSpeed = 5.0f;

    private Camera _mainCamera;

    public MouseTargetPosition(PlayerController.BoneConfig[] bodyBones, ref PlayerParents parents)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _mainCamera = Camera.main;

        _bodyBones = bodyBones;
    }

    public void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.visible)
            Cursor.visible = false;

        Vector3 cameraForward = _mainCamera.transform.forward;
        Quaternion baseTargetRotation = Quaternion.LookRotation(cameraForward.normalized);

        foreach (PlayerController.BoneConfig config in _bodyBones)
        {
            if (config.bone != null)
            {
                Quaternion localOffset = Quaternion.Euler(config.axisOffset);
                Quaternion correctedRotation = baseTargetRotation * localOffset;

                config.bone.rotation = Quaternion.Slerp
                (
                    config.bone.rotation,
                    correctedRotation,
                    config.rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}