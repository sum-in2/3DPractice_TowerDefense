using UnityEngine;

public class UpgradeBtn : MonoBehaviour
{
    public Upgrade upgrade;

    public void OnClickUpgradeBtn()
    {
        upgrade.UpgradeLevelAdder();
    }
}
