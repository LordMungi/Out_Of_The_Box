using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTargetPosition : MonoBehaviour
{
    private Camera _mainCamera;
    private Plane _groundPlane;
    
   private void Start()
    {
        Cursor.visible = false;
        
        _mainCamera = Camera.main;
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (Mouse.current == null) 
            return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Ray ray = _mainCamera.ScreenPointToRay(mouseScreenPosition);

        if (_groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 hitPoint = ray.GetPoint(rayDistance);
            Vector3 lookDirection = hitPoint - transform.position;
            
            lookDirection.y = 0; 

            if (lookDirection.sqrMagnitude > 0.05f)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
}
