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

        // 클릭 했을때, UIState를 할당 받은 패널을 온오프 해야함.
        // 근데 여기 스팟이 타워인지 타워스팟인지에 따라서 다르게 띄워야 할거같긴한데?
        // 하지만 타워를 설치하면 여기 스크립트는 없을꺼니까 아마
        // 여기엔 towerlist 호출하면 될듯?
        matInstance.color = selectedColor;
    }

    public void OnDeselect()
    {
        matInstance.color = defaultColor;
    }
}
