using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "TowerDefense/GameState")]
public class GameState : ScriptableObject
{
    [field: SerializeField] public int StageLevel { get; set; } = 1;
    [field: SerializeField] public int Gold { get; set; } = 500;
    [field: SerializeField] public int Lives { get; set; } = 20;
    [field: SerializeField] public bool IsPaused { get; set; }
}