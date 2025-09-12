using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// TowerSpot이 클릭된 채로 버튼에 Hover하면 중앙 상태 UI를 업데이트하는 클래스입니다.
/// </summary>
public class TowerBtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Action<TowerType, AttackStats> OnTowerHover;
    public static Action OnTowerHoverExit;

    private TowerType towerType;
    private AttackStats attackStats;

    void Start()
    {
        towerType = gameObject.GetComponent<PlaceTowerBtn>().towerPrefab.GetComponent<BaseTower>().towerType;
        attackStats = SOManager.Instance.GetTowerRuntimeStat(towerType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnTowerHover?.Invoke(towerType, attackStats);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnTowerHoverExit?.Invoke();
    }
}
