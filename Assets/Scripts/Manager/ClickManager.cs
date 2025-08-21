using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickManager : Singleton<ClickManager>
{
    private Camera mainCamera;
    private IClickable nowClickObject;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 mousePos = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        nowClickObject = null;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            nowClickObject = hit.collider.GetComponent<IClickable>();

            // TODO: 클릭된 오브젝트에 따라서 상태 UI 변경

            if (nowClickObject != null)
            {
                nowClickObject.OnSelect();
                UIManager.Instance.ChangeState(nowClickObject.CurrentState);
            }
            else
                UIManager.Instance.ChangeState(StateType.None);
            DeselectAllExcept(nowClickObject);
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
