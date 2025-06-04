using UnityEngine;

public class TowerSpot : MonoBehaviour, IClickable
{
    private Renderer rend;
    private Material matInstance;

    public Color defaultColor = Color.green;
    public Color selectedColor = Color.yellow;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        matInstance = rend.material;
        matInstance.color = defaultColor;
    }

    public void OnSelect()
    {
        // TODO : UIManager에 타워 리스트 구현
        matInstance.color = selectedColor;
    }

    public void OnDeselect()
    {
        matInstance.color = defaultColor;
    }
}
