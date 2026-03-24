# UI 구조 문서

## 계층 구조

```
UIManager (Singleton)
├── PopupManager
│   └── UIPopup 목록 (팝업별 비활성 GameObject, PopupType enum으로 식별)
└── StateManager
    ├── towerButtons[]       (타워 배치 버튼 그룹)
    ├── upgradeButtons[]     (업그레이드 버튼 그룹)
    ├── centerPanel          (중앙 패널)
    ├── rightPanel           (우측 패널)
    └── TowerInfoUI          (타워 스탯 정보 패널)
```

---

## 스크립트 목록

### 매니저

| 파일                      | 역할                                                         |
| ------------------------- | ------------------------------------------------------------ |
| `Manager/UIManager.cs`    | PopupManager·StateManager 위임 코디네이터 (Singleton)        |
| `Manager/StateManager.cs` | None→TowerSpotSelect→TowerSelect 상태 머신, 버튼 ON/OFF 제어 |
| `Manager/PopupManager.cs` | Stack 기반 팝업 레이어링 (Canvas sortingOrder 10부터 증가)   |

### UI 베이스 / 컴포넌트

| 파일                | 역할                                                                                       |
| ------------------- | ------------------------------------------------------------------------------------------ |
| `UI/UIBase.cs`      | Show / Hide / SetSortingOrder 추상 기반 클래스                                             |
| `UI/UIPopup.cs`     | UIBase 구체 구현 — GameObject 이름으로 PopupType enum 파싱                                 |
| `UI/UIState.cs`     | UIBase 구체 구현 — GameObject 이름으로 StateType enum 파싱                                 |
| `UI/UIEvents.cs`    | `OnStateChangeRequested` 정적 이벤트 허브                                                  |
| `UI/TowerInfoUI.cs` | 타워 스탯 패널 (TMP 필드 8개: 이름·설명·공격력·사거리·공속·무시방어·치명타확률·치명타배율) |
| `UI/GoldUI.cs`      | `GameManager.OnGoldChanged` 구독 → 골드 텍스트 갱신                                        |

### 버튼 핸들러 (`Btn/`)

| 파일                          | 역할                                                     |
| ----------------------------- | -------------------------------------------------------- |
| `Btn/PlaceTowerBtn.cs`        | 타워 배치 버튼 → `ClickManager.PlaceTower()`             |
| `Btn/UpgradeBtn.cs`           | 업그레이드 버튼 → SOManager 경유 전역 업그레이드 적용    |
| `Btn/TowerBtnHover.cs`        | 호버 시 `OnTowerHover` 정적 이벤트 발행                  |
| `Btn/PreviewRangeBtnHover.cs` | 호버 시 타워 사거리 시각화                               |
| `Btn/StageStartBtn.cs`        | `GameManager.NextStage()` 호출                           |
| `UI/PopupUIClick.cs`          | 팝업 열기 버튼 핸들러                                    |
| `UI/PopupUIQuit.cs`           | 팝업 닫기 버튼 핸들러 (실제 클래스명: `PopupQuitButton`) |

---

## 상태 머신 (StateManager)

| 상태              | 버튼 표시                              | 패널                              |
| ----------------- | -------------------------------------- | --------------------------------- |
| `None`            | 모두 숨김                              | 모두 숨김                         |
| `TowerSpotSelect` | towerButtons 표시, upgradeButtons 숨김 | centerPanel·rightPanel 표시       |
| `TowerSelect`     | upgradeButtons 표시, towerButtons 숨김 | TowerInfoUI에 선택 타워 정보 표시 |

---

## 주요 이벤트 흐름

### 상태 변경

```
ClickManager.nowClickObject 변경
  → UIEvents.OnStateChangeRequested 발행
    → UIManager.ChangeState() → StateManager.SetState()
    → TowerInfoUI.HandleStateChange()
```

### 타워 업그레이드

```
UpgradeBtn.OnClickUpgradeBtn()
  → Upgrade.UpgradeLevelAdder()
  → SOManager.OnTowerUpgraded 발행
    → StateManager → TowerInfoUI 갱신
    → TowerManager → 해당 타입 전체 타워 스탯 갱신
```

### 팝업 열기/닫기

```
PopupUIClick → UIManager.ShowPopup()
  → PopupManager.ShowPopupUI()
    → UIPopup.Show() + Stack push + sortingOrder 증가

PopupQuitButton → UIManager.ClosePopup()
  → PopupManager.ClosePopupUI()
    → Stack top 확인 → UIPopup.Hide() + Stack pop + sortingOrder 감소
```

### 골드 표시

```
GameManager.Gold 변경 → OnGoldChanged 발행 → GoldUI.UpdateGoldText()
```

### 타워 호버 프리뷰

```
TowerBtnHover.OnPointerEnter()
  → OnTowerHover 정적 이벤트 발행
    → StateManager.HandleTowerHover() → TowerInfoUI 임시 표시
    → TowerInfoUI.HandleTowerHover() → UpdateTowerInfo()
PreviewRangeBtnHover.OnPointerEnter() → 사거리 시각화 오브젝트 활성화
```

---

## 열거형

```csharp
enum StateType  { None, TowerSelect, TowerSpotSelect }
enum PopupType  { Option }
```

> 새 팝업 추가 시: `PopupType` enum에 항목 추가 → 씬에 UIPopup GameObject 배치 (이름을 enum 이름과 동일하게) → `PopupManager.popupEntries`에 등록
