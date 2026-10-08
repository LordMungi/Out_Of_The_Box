using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCam : MonoBehaviour
{
    private float _flySpeed = 10.0f;
    private float _lookSpeed = 0.2f;
    
    public static bool IsActive { get; private set; } = false;
    
    private Key _toggleKey = Key.F1;

    private CinemachineCamera _freeCam;

    private float _pitch = 0f;
    private float _yaw = 0f;

    private void Awake()
    {
        _freeCam = GetComponent<CinemachineCamera>();
        _freeCam.enabled = false;
        
        Vector3 angles = transform.eulerAngles;
        _pitch = angles.x;
        _yaw = angles.y;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current[_toggleKey].wasPressedThisFrame)
        {
            IsActive = !IsActive;
            _freeCam.enabled = IsActive;
            
            if (IsActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
        if (IsActive)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            _yaw += mouseDelta.x * _lookSpeed;
            _pitch -= mouseDelta.y * _lookSpeed;

            _pitch = Mathf.Clamp(_pitch, -89.0f, 89.0f);
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }

        if (Keyboard.current != null)
        {
            float moveX = 0.0f;
            float moveY = 0.0f;
            float moveZ = 0.0f;

            if (Keyboard.current.dKey.isPressed)
                moveX += 1.0f;
            if (Keyboard.current.aKey.isPressed)
                moveX -= 1.0f;

            if (Keyboard.current.wKey.isPressed) 
                moveZ += 1.0f;;
            if (Keyboard.current.sKey.isPressed) 
                moveZ -= 1.0f;;

            if (Keyboard.current.eKey.isPressed) 
                moveY = 1.0f;
            if (Keyboard.current.qKey.isPressed) 
                moveY = -1.0f;

            Vector3 move = new Vector3(moveX, moveY, moveZ);
            
            if (move.magnitude > 1.0f)
                move.Normalize();

            transform.Translate(move * _flySpeed * Time.deltaTime, Space.Self);
        }
    }
}