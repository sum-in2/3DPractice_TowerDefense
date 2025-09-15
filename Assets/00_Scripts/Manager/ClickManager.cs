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
                    UIEvents.OnStateChangeRequested?.Invoke(_nowClickObject.currentState);
                else
                    UIEvents.OnStateChangeRequested?.Invoke(StateType.None);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.OnPointerClick += HandleClick;
    }

    private void OnDisable()
    {
        InputManager.OnPointerClick -= HandleClick;
    }

    private void HandleClick(Vector2 mousePos)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        nowClickObject = null;

        if (Physics.Raycast(ray, out RaycastHit hit))
            nowClickObject = hit.collider.GetComponent<IClickable>();

        if (nowClickObject != null)
            nowClickObject.OnSelect();

        DeselectAllExcept(nowClickObject);
    }

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

        if (myTowerSpot && myTowerSpot.CanPlaceTower())
        {
            GameObject towerGameObject = myTowerSpot.PlaceTower(towerPrefab);

            if (towerGameObject != null)
            {
                BaseTower placedTower = towerGameObject.GetComponent<BaseTower>();
                nowClickObject = placedTower;

                if (UIManager.Instance != null && UIManager.Instance.stateManager != null)
                    UIManager.Instance.stateManager.OnTowerPlaced(placedTower);
            }
        }
    }
}
