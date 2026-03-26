using System.Collections.Generic;
using UnityEngine;

public class UpgradeBtn : MonoBehaviour
{
    public UpgradeType upgradeType;

    public void OnClickUpgradeBtn()
    {
        BaseTower selectedTower = ClickManager.Instance.nowClickObject as BaseTower;
        if (selectedTower == null)
        {
            Debug.LogWarning("클릭된 타워 없음");
            return;
        }

        List<Upgrade> upgrades = SOManager.Instance.GetTowerUpgradeSOList(selectedTower.towerType);
        if (upgrades == null || upgrades.Count == 0)
        {
            Debug.LogWarning("해당하는 타워 업그레이드 SO가 없음 > TowerName: " + selectedTower.name + " UpgradeType: " + upgradeType);
            return;
        }

        Upgrade targetUpgrade = null;
        foreach (Upgrade upgradeData in upgrades)
        {
            if (upgradeData.UpgradeType == this.upgradeType)
            {
                targetUpgrade = upgradeData;
                break;
            }
        }

        if (targetUpgrade == null)
        {
            Debug.LogWarning("업그레이드 타입을 찾을 수 없습니다");
            return;
        }

        if (!targetUpgrade.TryUpgrade())
            Debug.Log("골드 부족");
    }
}
