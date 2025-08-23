using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private Stack<UIPopup> popupStack = new Stack<UIPopup>();
    private int order = 10;

    [System.Serializable]
    public class PopupEntry
    {
        public PopupType popupType;
        public UIPopup popupObject; // 씬에 미리 배치된 비활성화된 오브젝트
    }
    public List<PopupEntry> popupEntries;
    private Dictionary<PopupType, UIPopup> popupDict;

    protected void Awake()
    {
        popupDict = new Dictionary<PopupType, UIPopup>();
        foreach (var entry in popupEntries)
        {
            if (entry.popupObject != null && !popupDict.ContainsKey(entry.popupType))
                popupDict.Add(entry.popupType, entry.popupObject);
        }
    }

    public void ShowPopupUI(PopupType type)
    {
        if (!TryGetPopup(type, out UIPopup popup))
            return;

        if (popup.gameObject.activeSelf)
        {
            Debug.LogWarning($"{type} 팝업이 이미 열려 있습니다.");
            return;
        }

        popup.Show();
        popup.SetSortingOrder(order++);
        popupStack.Push(popup);

        return;
    }

    public void ClosePopupUI(PopupType type)
    {
        if (!TryGetPopup(type, out UIPopup popup))
            return;

        if (popupStack.Count == 0)
            return;
        if (popupStack.Peek() != popup)
            return;

        UIPopup topPopup = popupStack.Pop();
        topPopup.Hide();
        order--;
    }

    private bool TryGetPopup(PopupType type, out UIPopup popup)
    {
        if (!popupDict.TryGetValue(type, out popup) || popup == null)
        {
            Debug.LogError($"{type} 팝업이 등록되어 있지 않습니다.");
            return false;
        }
        return true;
    }
}
