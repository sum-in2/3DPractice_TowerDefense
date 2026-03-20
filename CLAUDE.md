# CLAUDE.md

이 파일은 Claude Code(claude.ai/code)가 이 저장소에서 작업할 때 참고하는 가이드입니다.

## 프로젝트 개요

Unity 6.0.3.2f1 기반 3D 타워 디펜스 게임 (Windows 64-bit). C# 7.3, .NET Standard 2.1. Unity Hub에서 프로젝트를 열고 Unity Editor의 Play 버튼으로 실행. 메인 씬: `Assets/Scenes/MazingTD.unity`.

## 아키텍처

### 매니저 레이어 (`Singleton<T>` 기반 싱글톤)

모든 핵심 매니저는 `Singleton<T>` (`Assets/00_Scripts/Util/Singleton.cs`)를 상속하며 C# 이벤트로 통신한다.

| 매니저 | 역할 |
|---|---|
| `GameManager` | 게임 상태, 골드 경제, 스테이지 진행 관리; `OnGoldChanged` 이벤트 발행 |
| `TowerManager` | 타워 레지스트리; 특정 타입의 모든 타워에 전역 업그레이드 적용 |
| `SOManager` | ScriptableObject 데이터 접근; `ITowerUpgradeNotifier` 구현; 기본/런타임 타워 스탯 및 테크 잠금 상태 관리 |
| `StateManager` | UI 상태 머신 (`None → TowerSpotSelect → TowerSelect`); 버튼 표시/숨김 |
| `UIManager` | 상위 UI 코디네이터; `PopupManager`와 `StateManager`의 부모 |
| `StageManager` | 적 스폰 라이프사이클 관리; `StageState` (Idle/Playing/Completed) 추적 |
| `GridManager` | 시작 시 5×5 `TowerSpot` 그리드 절차적 생성; 웨이포인트 초기화 |
| `ClickManager` | `IClickable` 오브젝트에 레이캐스트; 선택 상태 관리; `PlaceTower()` 호출 |
| `InputManager` | 마우스 원시 입력 처리; UI 클릭 필터링; `OnPointerClick` 이벤트 발행 |
| `ObjectPoolManager` | 적과 발사체를 위한 제네릭 오브젝트 풀 |

### 주요 데이터 흐름

**입력 → 동작:**
```
InputManager.OnPointerClick → ClickManager → IClickable.OnSelect/OnDeselect → UIEvents.OnStateChangeRequested → StateManager
```

**타워 배치:**
```
TowerSpot (IClickable) 선택 → PlaceTowerBtn → ClickManager.PlaceTower() → TowerSpot.PlaceTower() → 타워가 TowerManager에 등록
```

**타워 공격 루프 (코루틴):**
```
BaseTower.AttackRoutine() → Physics.OverlapSphere() → TowerAttackBehaviorFactory → IAttackBehavior.Attack() → ObjectPoolManager가 Projectile 스폰 → Projectile.HitTarget() → Enemy.TakeDamage() → Enemy.Die() → 골드 보상 + 풀 반환
```

**업그레이드 흐름:**
```
UpgradeBtn → Upgrade SO.UpgradeLevelAdder() → SOManager.ApplyGlobalUpgrade() → ITowerUpgradeNotifier.OnTowerUpgraded → TowerManager가 해당 타입 전체 타워 갱신 → BaseTower.RefreshCurrentStats()
```

### 타워 시스템

- `BaseTower` (추상 클래스): `Physics.OverlapSphere`로 타겟 탐지, `AttackRoutine` 코루틴 실행, 개별/전역 업그레이드 적용
- 구체 타워 클래스: `BasicTower`, `EnergyTower`, `PlasmaChainTower`
- 공격 행동은 `TowerType` enum 기반으로 `TowerAttackBehaviorFactory`에서 생성 — 새 타워 타입 추가 시 여기에 등록
- `AttackStats` 구조체: `attackPower`, `range`, `attackSpeed`, `ignoreDefense`, `critChance`, `critDamage`

### ScriptableObject 데이터 레이어 (`Assets/02_ScriptableObjectScripts/`)

- `TowerStat` — 타워 타입별 기본 스탯
- `Upgrade` — 업그레이드 정의 (타입, 증가량, 레벨)
- `StageDataSO` — `StageInfo` 목록 (몬스터 이름, HP, 보상, 스폰 간격, 수)
- `GameState` — 런타임 상태 (골드, 생명, 스테이지 레벨, EXP)
- `EXPTable` — 레벨별 EXP 임계값
- `Tech` — 테크 트리 잠금 해제 정의

### 주요 인터페이스 (`Assets/00_Scripts/Interface/`)

- `IClickable` — `currentState`, `OnSelect()`, `OnDeselect()` — `TowerSpot`과 `BaseTower`가 구현
- `IAttackBehavior` — `Attack()` — 모든 공격 행동 클래스가 구현
- `ITowerUpgradeNotifier` — `SOManager`가 구현하는 업그레이드 이벤트 계약

### 열거형 (`Assets/00_Scripts/EnumList.cs`)

중앙 열거형 파일: `TowerType`, `UpgradeType`, `StateType`, `StageState`, `PopupType`. 새 타워 타입은 여기에 먼저 추가한 뒤 `TowerAttackBehaviorFactory`를 수정한다.

## 새 타워 타입 추가 방법

1. `EnumList.cs`의 `TowerType` enum에 항목 추가
2. `BaseTower`를 상속하는 타워 MonoBehaviour 생성
3. `IAttackBehavior`를 구현하는 공격 행동 클래스 추가
4. `TowerAttackBehaviorFactory.cs`에 등록
5. 새 타입에 대한 `TowerStat` ScriptableObject 에셋 생성
6. 풀링 발사체를 사용하는 경우 프리팹 생성 후 `ObjectPoolManager`에 추가
