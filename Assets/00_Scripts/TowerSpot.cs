using Unity.VisualScripting;
using UnityEngine;

public class TowerSpot : MonoBehaviour, IClickable
{
    private Renderer rend;
    private BoxCollider col;
    private Material matInstance;

    public Color defaultColor = Color.green;
    public Color selectedColor = Color.yellow;

    public StateType CurrentState { get; private set; } = StateType.Tower;

    [Header("타워 상태")]
    public bool isOccupied = false; // 타워 설치 여부
    public int gridX, gridY;        // 그리드 인덱스(옵션)

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<BoxCollider>();

        matInstance = rend.material;
        matInstance.color = defaultColor;
    }

    public void OnSelect()
    {
        // TODO : UIManager에 타워 리스트 구현

        // UIManager에 타워 리스트 창 띄우기
        // 타워 설치 가능하고(isOccupied==false) 선택한 경우엔만 타워 설치 UI 실행

        matInstance.color = selectedColor;
    }

    public void OnDeselect()
    {
        matInstance.color = defaultColor;
    }

    public bool CanPlaceTower()
    {
        return !isOccupied;
    }

    public void SetIndex(int x, int y)
    {
        gridX = x;
        gridY = y;
    }

    public void PlaceTower(GameObject towerPrefab)
    {
        if (CanPlaceTower())
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            isOccupied = true;
            col.enabled = false;
            rend.enabled = false;
        }
    }
}
