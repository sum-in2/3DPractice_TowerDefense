using UnityEngine;

public class TowerSpot : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        // TODO : UIManager에 타워 리스트 구현
        // 얘 머터리얼 색 바꾸기? (색을 할지 투명도를 할지 고민 중)
        Debug.Log("TowerSpot has clicked");
    }
}
