using UnityEngine;
using System;

/// <summary>
/// 중앙 하단 상태 UI의 행동을 처리합니다
/// </summary>

public class UIState : UIBase
{
    public StateType popupType { get; private set; }
    protected override void Init()
    {
        if (Enum.TryParse<StateType>(transform.name, out StateType result))
        {
            popupType = result;
        }
        else
        {
            Debug.Log("변환 실패");
        }
    }

    //TODO: TowerSpot > BaseTower > 업그레이드 갈래 순차적으로 열리는 방안 
}