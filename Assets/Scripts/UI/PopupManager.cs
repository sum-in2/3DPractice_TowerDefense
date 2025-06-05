using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopupManager : Singleton<PopupManager>
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

    protected override void Awake()
    {

        popupDict = new Dictionary<PopupType, UIPopup>();
        foreach (var entry in popupEntries)
        {
            if (entry.popupObject != null && !popupDict.ContainsKey(entry.popupType))
                popupDict.Add(entry.popupType, entry.popupObject);
        }
    }

    public T ShowPopupUI<T>(PopupType type) where T : UIPopup
    {
        if (!popupDict.TryGetValue(type, out UIPopup popup) || popup == null)
        {
            Debug.LogError($"{type} 팝업이 등록되어 있지 않습니다.");
            return null;
        }

        if (popup.gameObject.activeSelf)
        {
            Debug.LogWarning($"{type} 팝업이 이미 열려 있습니다.");
            return popup as T;
        }

        popup.gameObject.SetActive(true);
        popup.SetSortingOrder(order++);
        popupStack.Push(popup);

        return popup as T;
    }

    public void ClosePopupUI(UIPopup popup)
    {
        if (popupStack.Count == 0)
            return;
        if (popupStack.Peek() != popup)
            return;

        UIPopup topPopup = popupStack.Pop();
        topPopup.gameObject.SetActive(false);
        order--;
    }
}
