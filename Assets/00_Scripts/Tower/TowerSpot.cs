using Unity.VisualScripting;
using UnityEngine;

public class TowerSpot : MonoBehaviour, IClickable
{
    private Transform parent;
    private Renderer rend;
    private BoxCollider col;
    private Material matInstance;
    private VertexLineDrawer vertexLineDrawer;

    public Color defaultColor = Color.green;
    public Color selectedColor = Color.yellow;

    public StateType currentState { get; private set; } = StateType.Tower;

    [Header("타워 상태")]
    public bool isOccupied = false; // 타워 설치 여부
    public int gridX, gridY;        // 그리드 인덱스(옵션)

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<BoxCollider>();
        vertexLineDrawer = GetComponent<VertexLineDrawer>();

        matInstance = rend.material;
        matInstance.color = defaultColor;
    }

    public void OnSelect()
    {
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

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    public GameObject PlaceTower(GameObject towerPrefab)
    {
        if (CanPlaceTower())
        {
            isOccupied = true;
            col.enabled = false;
            rend.enabled = false;
            vertexLineDrawer.enabled = false;
            return Instantiate(towerPrefab, transform.position, Quaternion.identity, parent);
        }

        return null;
    }
}
