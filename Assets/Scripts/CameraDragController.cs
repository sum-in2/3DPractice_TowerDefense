using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDragController : MonoBehaviour
{
    public float dragSpeed = 0.1f;
    private Camera cam;
    private bool isDragging = false;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDragging = true;
        }
        else if (context.canceled)
        {
            isDragging = false;
        }
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
}
