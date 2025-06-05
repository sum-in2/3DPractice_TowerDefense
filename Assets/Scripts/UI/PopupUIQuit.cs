using UnityEngine;
using UnityEngine.UI;

public class PopupQuitButton : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponent<Button>();

        if (button == null)
            button = GetComponentInParent<Button>();

        if (button != null)
            button.onClick.AddListener(HidePopup);
        else
            Debug.LogWarning($"{gameObject.name}에 Button 없음");
    }

    public void HidePopup()
    {
        UIPopup popup = GetComponentInParent<UIPopup>();
        if (popup == null)
        {
            Debug.LogError("부모에 UIPopup이 없습니다.");
            return;
        }

        PopupType type = popup.popupType;
        UIManager.Instance.ClosePopup(type);
    }
}