using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TowerSpot이 클릭된 채로 버튼에 Hover하면 사거리를 보여주는 클래스입니다.
/// </summary>
public class PreviewRangeBtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    float towerRange;
    TowerType towerType;
    GameObject nowTowerSpot;

    void RangeInit()
    {
        towerType = gameObject.GetComponent<PlaceTowerBtn>().towerPrefab.GetComponent<BaseTower>().towerType;
        AttackStats attackStat = SOManager.Instance.GetTowerRuntimeStat(towerType);
        towerRange = attackStat.range;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IClickable clickable = ClickManager.Instance.nowClickObject;
        if (clickable != null)
        {
            RangeInit();
            nowTowerSpot = (clickable as TowerSpot)?.gameObject;
            nowTowerSpot.GetComponentInChildren<PreviewRange>(true)?.SetRangeObjectState(towerRange, true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        nowTowerSpot.GetComponentInChildren<PreviewRange>()?.SetRangeObjectState(0, false);
        nowTowerSpot = null;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        nowTowerSpot.GetComponentInChildren<PreviewRange>()?.SetRangeObjectState(0, false);
        nowTowerSpot = null;
    }
}