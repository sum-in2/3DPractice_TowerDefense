using UnityEngine;

/// <summary>
/// 타워 종류 해금 SO
/// </summary>

[CreateAssetMenu(menuName = "Game/Tech")]
public class Tech : ScriptableObject
{
    public TowerType techType;
    public int level;
    public string description;
}
