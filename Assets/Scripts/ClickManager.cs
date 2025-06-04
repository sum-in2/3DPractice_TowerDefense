using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
                clickable.OnSelect();
                DeselectAllExcept(clickable);
            }
        }
    }

    private void DeselectAllExcept(IClickable selected)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            IClickable[] clickables = root.GetComponentsInChildren<IClickable>(true); // 비활성 포함
            foreach (IClickable clickable in clickables)
            {
                if (clickable != selected)
                    clickable.OnDeselect();
            }
        }
    }
}
