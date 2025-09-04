using System.Collections.Generic;
using UnityEngine;

public class UpgradeBtn : MonoBehaviour
{
    public UpgradeType upgrade;

    public void OnClickUpgradeBtn()
    {
        // TODO : 선택된 타워 타입 > 해당 타입에 해당하는 SO 레벨 업 시키는 메서드
        // 연결돼야 하는 매니저 SO매니저, 클릭매니저
        // 클릭매니저 nowclick 어쩌구 > 캐스팅(basetower) > towerType
        // SO매니저 > GetTowerUpgradeSOList(towerType) > 리스트 중 UpgradeType에 일치하는 업그레이드 레벨업
        // upgrade.UpgradeLevelAdder();

        BaseTower towerType = ClickManager.Instance.nowClickObject as BaseTower;
        if (towerType == null)
        {
            Debug.LogWarning("클릭된 타워 없음");
            return;
        }

        List<Upgrade> upgrades = SOManager.Instance.GetTowerUpgradeSOList(towerType.towerType);
        if (upgrades == null)
        {
            Debug.LogWarning("해당하는 타워 업그레이드 SO가 없음 > TowerName: " + towerType.name + " UpgradeType: " + upgrade);
            return;
        }

        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.upgradeType == this.upgrade)
                upgrade.UpgradeLevelAdder();
        }
    }
}
