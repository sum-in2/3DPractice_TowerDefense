using UnityEngine;
using UnityEngine.UI;

public class PlaceTowerBtn : MonoBehaviour
{
    public GameObject towerPrefab;

    public void OnClickPlaceTowerBtn()
    {
        ClickManager.Instance.PlaceTower(towerPrefab);
    }
}