using UnityEngine;
using System;

public class UIPopup : UIBase
{
    public PopupType popupType { get; private set; }
    protected override void Init()
    {
        if (Enum.TryParse<PopupType>(transform.name, out PopupType result))
        {
            popupType = result;
        }
        else
        {
            Debug.Log("변환 실패");
        }
    }
}