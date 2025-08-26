using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickManager : Singleton<ClickManager>
{
    private Camera mainCamera;
    private IClickable _nowClickObject;
    public IClickable nowClickObject
    {
        get => _nowClickObject;
        private set
        {
            if (_nowClickObject != value)
            {
                _nowClickObject = value;
                if (_nowClickObject != null)
                    UIManager.Instance.ChangeState(_nowClickObject.CurrentState);
                else
                    UIManager.Instance.ChangeState(StateType.None);
            }
        }
    }

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
        }

        if (nowClickObject != null)
            nowClickObject.OnSelect();

        DeselectAllExcept(nowClickObject);
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

    public void PlaceTower(GameObject towerPrefab)
    {
        TowerSpot myTowerSpot = nowClickObject as TowerSpot;
        if (myTowerSpot)
        {
            nowClickObject = myTowerSpot.PlaceTower(towerPrefab).GetComponent<IClickable>();
        }
    }
}
