using UnityEngine;

/// <summary>
/// 타워 수치 업그레이드 SO
/// </summary>

[CreateAssetMenu(menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public float increaseAmount;
    public string description;
}
