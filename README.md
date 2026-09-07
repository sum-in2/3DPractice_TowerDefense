# 3D Practice Tower Defense

Unity 6(URP) 기반의 3D 타워 디펜스 프로젝트입니다. 그리드에 타워를 배치하고, 타워별로 다른 공격 로직(단일 투사체 / 에너지 / 플라즈마 체인)을 전략 패턴으로 분리해 처리합니다.

> 스크린샷 / 플레이 GIF 추가 예정

## 핵심 구현: 타워 공격 전략 패턴

타워 종류(`TowerType`)마다 발사체 생성 방식, 타겟팅, 이펙트가 다릅니다. `BaseTower`가 구체 로직을 직접 알지 않도록 `IAttackBehavior` 인터페이스와 팩토리로 분리했습니다.

```csharp
public interface IAttackBehavior
{
    void Attack(BaseTower tower);
}

public static class TowerAttackBehaviorFactory
{
    public static IAttackBehavior Create(TowerType type) => type switch
    {
        TowerType.Basic => new BasicTowerAttack(),
        TowerType.Energy => new EnergyTowerAttack(),
        TowerType.PlasmaChain => new PlasmaChainTowerAttack(),
        _ => null,
    };
}
```

새 타워를 추가할 때 `BaseTower`나 기존 공격 로직을 건드리지 않고 `IAttackBehavior` 구현체와 `enum` 케이스만 추가하면 됩니다.

## 주요 기능

- 그리드 기반 타워 배치 (`GridManager`, `TowerSpot`)
- 타워별 전략 패턴 공격 로직 3종 (단일 사격 / 에너지 / 플라즈마 체인)
- SO 기반 개별/전역 업그레이드 시스템 (`SOManager`, `IndividualUpgrade`)
- 제네릭 오브젝트 풀링으로 투사체/적 재사용
- 이벤트 기반 UI 상태 전환 (타워 배치 ↔ 업그레이드 패널)

## 핵심 구현 상세

### 1. 제네릭 오브젝트 풀링

투사체와 적 스폰이 매 프레임 반복되는 만큼, `Instantiate`/`Destroy` 대신 타입 무관하게 재사용 가능한 풀을 만들었습니다.

```csharp
public class ObjectPool<T> where T : Component
{
    private Queue<T> objects = new Queue<T>();

    public T GetObject()
    {
        if (objects.Count == 0)
            objects.Enqueue(GameObject.Instantiate(prefab, parent));

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }
}
```

`ObjectPoolManager`가 프리팹별로 `ObjectPool<T>` 인스턴스를 딕셔너리로 관리하고, 등록되지 않은 프리팹은 요청 시점에 자동으로 풀을 생성하도록 해 사용하는 쪽(타워, 스포너)이 풀 존재 여부를 신경 쓰지 않게 했습니다.

### 2. 기본 스탯 / 런타임 스탯 분리

업그레이드는 타워 인스턴스 하나에만 적용되는 개별 업그레이드와, 같은 타입 전체에 적용되는 전역 업그레이드 두 종류가 있습니다. `SOManager`는 SO에 저장된 기본값(`defaultStatDict`)을 건드리지 않고, 별도의 런타임 딕셔너리(`runtimeStatDict`)에 복사해 전역 업그레이드를 적용합니다.

```csharp
void CopyAllDefaultToRuntime()
{
    foreach (var kvp in defaultStatDict)
        runtimeStatDict[kvp.Key] = new AttackStats(kvp.Value);
}

public void ApplyGlobalUpgrade(TowerType type, UpgradeType upgradeType, float amount)
{
    runtimeStatDict[type].UpgradeStat(upgradeType, amount);
    OnTowerUpgraded?.Invoke(type);
}
```

타워는 `Start()` 시점에 이 런타임 스탯을 복사해 개별 업그레이드(`RefreshCurrentStats`)를 얹는 구조라, 에디터에 저장된 SO 원본 데이터는 플레이 중에도 항상 깨끗하게 유지됩니다.

### 3. 이벤트 기반 상태 전환

타워 배치 패널과 업그레이드 패널을 전환할 때 각 UI 컴포넌트가 서로를 직접 참조하지 않도록, `StateManager`가 상태 변경을 이벤트로 발행하고 UI/버튼 쪽이 구독합니다.

```csharp
public event StateChangedHandler OnStateChanged;

public StateType CurrentState
{
    set
    {
        if (currentState != value)
        {
            currentState = value;
            OnStateChanged?.Invoke(currentState);
        }
    }
}
```

`TowerBtnHover`, `SOManager`의 업그레이드 이벤트 등 여러 소스가 `StateManager`를 거쳐 UI 갱신 한 곳으로 모이기 때문에, 패널 로직을 바꿀 때 이벤트 구독부만 수정하면 됩니다.

## 아키텍처

```
Assets
├── 00_Scripts
│   ├── Manager        # Singleton<T> 기반 매니저 (Grid, Tower, Pool, State, SO, ...)
│   ├── Tower           # 타워 본체 + AttackStats
│   │   └── Projectile  # 투사체 (Basic / Chain / Energy)
│   ├── TowerAttack     # IAttackBehavior 구현체 + 팩토리
│   ├── Enemy           # 적 이동, HP, 리워드
│   ├── Interface       # IAttackBehavior, IClickable, ITowerUpgradeNotifier
│   ├── UI / Btn        # UI 상태, 버튼 상호작용
│   └── Util            # Singleton<T>, ObjectPool<T>, CameraController
├── 01_ScriptableObjects        # 타워 스탯 / 테크 / 업그레이드 데이터 에셋
└── 02_ScriptableObjectScripts  # 위 SO들의 클래스 정의
```

- **매니저 레이어**: `Singleton<T>` 제네릭 베이스 클래스로 `GridManager`, `TowerManager`, `ObjectPoolManager`, `SOManager` 등을 통일된 방식으로 접근
- **데이터 레이어**: 타워 스탯/테크/업그레이드를 ScriptableObject로 분리해 기획 데이터와 로직 코드를 분리
- **공격 로직 레이어**: `IAttackBehavior` 전략 패턴으로 타워별 공격 방식을 독립적으로 확장

## 기술 스택

- Unity 6000.3.2f1 (URP)
- Universal Render Pipeline, VFX Graph
- Input System
- C# (Coroutine 기반 공격 루틴, 이벤트 기반 상태 관리)

## 알려진 제한 / 다음 작업

- 투사체 데미지 계산에서 방어력 관통, 치명타 적용 로직 미구현 (`Projectile.SetDamage` TODO)
- 업그레이드 세이브/로드 미구현 (`SOManager.UpgradeSOLevelInit` TODO)
