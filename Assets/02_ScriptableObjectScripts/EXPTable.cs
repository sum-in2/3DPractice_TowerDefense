using UnityEngine;

[CreateAssetMenu(fileName = "EXPTable", menuName = "TowerDefense/EXPTable")]
public class EXPTable : ScriptableObject
{
    [SerializeField] private int[] expTable;

    public int GetMaxEXP(int level)
    {
        if (level < expTable.Length)
            return expTable[level];
        return expTable[expTable.Length - 1];
    }
}