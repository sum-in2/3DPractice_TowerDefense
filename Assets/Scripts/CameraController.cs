using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 0.1f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;          // 휠 반응 속도
    public float zoomSmoothTime = 0.15f;  // 부드러운 러프 시간
    public float minZoom = 10f;
    public float maxZoom = 60f;

    private Camera cam;
    private bool isDragging = false;

    private float targetZoom;
    private float zoomVelocity = 0f;

    private void Awake()
    {
        cam = Camera.main;
        targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.started)
            isDragging = true;
        else if (context.canceled)
            isDragging = false;
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        if (!isDragging) return;

        if (context.performed)
        {
            Vector2 dragDelta = context.ReadValue<Vector2>();
            Vector3 move = new Vector3(-dragDelta.x, 0, -dragDelta.y) * dragSpeed * Time.deltaTime;
            cam.transform.Translate(move, Space.World);
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float scrollValue = context.ReadValue<Vector2>().y;
        if (Mathf.Abs(scrollValue) > 0.01f)
        {
            if (cam.orthographic)
            {
                targetZoom -= scrollValue * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
            else
            {
                targetZoom -= scrollValue * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
        }
    }

    private void LateUpdate()
    {
        if (cam.orthographic)
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);
        else
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref zoomVelocity, zoomSmoothTime);
    }
}
