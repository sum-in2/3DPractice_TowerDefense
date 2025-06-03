using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : Singleton<ClickManager>
{
    public void OnLClick(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue(); // 마우스 좌표 저장
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                clickable.OnClick();
            }
        }
    }
}
