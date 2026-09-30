using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 35.0f;

    [Header("Zoom")]
    [SerializeField] private float zoomStep = 3.0f;
    [SerializeField] private float minZoom = 18.0f;
    [SerializeField] private float maxZoom = 60.0f;

    [Header("Movement Bounds")]
    [SerializeField] private float minX = -45.0f;
    [SerializeField] private float maxX = 45.0f;
    [SerializeField] private float minZ = -75.0f;
    [SerializeField] private float maxZ = 60.0f;

    private Camera controlledCamera;

    private void Awake()
    {
        controlledCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1.0f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1.0f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1.0f;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1.0f;

        input = Vector2.ClampMagnitude(input, 1.0f);

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        Vector3 movement =
            (forward * input.y + right * input.x) *
            moveSpeed *
            Time.deltaTime;

        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        transform.position = newPosition;
    }

    private void HandleZoom()
    {
        if (Mouse.current == null)
        {
            return;
        }

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) < 0.01f)
        {
            return;
        }

        float direction = Mathf.Sign(scroll);

        controlledCamera.orthographicSize -= direction * zoomStep;

        controlledCamera.orthographicSize = Mathf.Clamp(
            controlledCamera.orthographicSize,
            minZoom,
            maxZoom
        );
    }
}