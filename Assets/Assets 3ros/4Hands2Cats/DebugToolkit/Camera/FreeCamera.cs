using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

namespace DebugToolkit.Freecam
{
    public class FreeCamera : MonoBehaviour
    {
        private DebugToolkit_IA _inputActions;

        [Header("Params"), Space]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float panSpeed = 10f;
        [SerializeField] private float zoomSpeed = 10f;

        [Header("Move Speed Panel"), Space]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI speedTMP;

        private float moveSpeedMod = 1;

        private bool _isFlying;
        private bool _isPanning;
        private float _sprint = 1f;
        private Coroutine moveSpeedCoroutine;


        private void Awake()
        {
            _inputActions = new DebugToolkit_IA();

            _inputActions.Debug.RightClick.performed += FlyMode;
            _inputActions.Debug.MiddleClick.performed += PanMode;
            _inputActions.Debug.Shift.performed += SprintMode;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void FlyMode(InputAction.CallbackContext context)
        {
            _isFlying = context.ReadValue<float>() != 0 ? true : false;
        }

        private void PanMode(InputAction.CallbackContext context)
        {
            _isPanning = context.ReadValue<float>() != 0 ? true : false;
        }

        private void SprintMode(InputAction.CallbackContext context)
        {
            _sprint = context.ReadValue<float>() != 0 ? 3 : 1;
        }

        void Update()
        {
            if (_isFlying)
            {
                RotateCamera();
                MoveCamera();
                MoveCameraVerticaly();
                MoveSpeedModifcation();
            }
            else
            {
                ZoomCamera();
            }

            if (_isPanning && !_isFlying)
                PanCamera();
        }

        void RotateCamera()
        {
            Vector3 deltaMouse = _inputActions.Debug.Look.ReadValue<Vector2>();
            float rotationX = deltaMouse.y * rotationSpeed * Time.unscaledDeltaTime;
            float rotationY = deltaMouse.x * rotationSpeed * Time.unscaledDeltaTime;
            transform.eulerAngles += new Vector3(-rotationX, rotationY, 0);
        }

        void PanCamera()
        {
            Vector3 deltaMouse = _inputActions.Debug.Look.ReadValue<Vector2>();
            Vector3 move = -transform.right * deltaMouse.x + -transform.up * deltaMouse.y;
            transform.position += move * panSpeed * Time.unscaledDeltaTime;
        }

        void MoveCamera()
        {
            Vector2 movement = _inputActions.Debug.Move.ReadValue<Vector2>();

            Vector3 forward = transform.forward;
            forward.Normalize();

            Vector3 right = transform.right;

            Vector3 move = (forward * movement.y + right * movement.x) * moveSpeed * moveSpeedMod * _sprint * Time.unscaledDeltaTime;
            transform.position += move;
        }

        void MoveCameraVerticaly()
        {
            float movement = _inputActions.Debug.VerticalMove.ReadValue<float>();

            Vector3 up = transform.up;

            Vector3 move = up * movement * moveSpeed * moveSpeedMod * _sprint * Time.unscaledDeltaTime;
            transform.position += move;
        }

        void ZoomCamera()
        {
            float scroll = _inputActions.Debug.ScrollWheel.ReadValue<Vector2>().y;
            if (scroll != 0f)
            {
                Vector3 forward = transform.forward;
                transform.position += forward * scroll * zoomSpeed * Time.unscaledDeltaTime;
            }
        }

        void MoveSpeedModifcation()
        {
            float scroll = _inputActions.Debug.ScrollWheel.ReadValue<Vector2>().y;
            if (scroll != 0f)
            {
                moveSpeedMod += scroll * 0.1f;
                moveSpeedMod = Mathf.Clamp(moveSpeedMod, 0.01f, 2f);

                if (moveSpeedCoroutine != null)
                {
                    StopCoroutine(moveSpeedCoroutine);
                }
                moveSpeedCoroutine = StartCoroutine(MoveSpeedPanel());

            }
        }

        IEnumerator MoveSpeedPanel()
        {
            if (moveSpeedMod < 0.1f)
            {
                speedTMP.text = $"{moveSpeedMod.ToString("F2").Replace('.', ',')}x";
            }
            else
            {
                speedTMP.text = $"{moveSpeedMod.ToString("0.00").Replace('.', ',')}x";
            }

            canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(1f);

            float duration = 0.5f;
            float elapsed = 0f;
            float startAlpha = canvasGroup.alpha;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            moveSpeedCoroutine = null;
        }

    }
}

