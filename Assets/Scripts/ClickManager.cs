using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClickManager : Singleton<ClickManager>
{
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = GetComponent<Camera>();
    }

    public void OnLClick(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        Vector2 mousePos = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                clickable.OnSelect();
                DeselectAllExcept(clickable);
            }
        }
    }


    /// <summary>
    /// 모든 IClickable 오브젝트의 선택을 해제합니다
    /// </summary>
    /// <param name="selected"></param>
    private void DeselectAllExcept(IClickable selected)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            IClickable[] clickables = root.GetComponentsInChildren<IClickable>(true);
            foreach (IClickable clickable in clickables)
            {
                if (clickable != selected)
                    clickable.OnDeselect();
            }
        }
    }
}
