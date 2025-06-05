using UnityEngine;
using UnityEngine.UI;

public class PopupUIClick : MonoBehaviour
{
    [SerializeField]
    private PopupType popupType;

    private void Start()
    {
        Button button = GetComponent<Button>();

        if (button == null)
            button = GetComponentInChildren<Button>();

        if (button != null)
            button.onClick.AddListener(ShowPopup);
        else
            Debug.LogWarning($"{gameObject.name}에 Button 없음");
    }

    public void ShowPopup()
    {
        UIManager.Instance.ShowPopup(popupType);
    }
}
