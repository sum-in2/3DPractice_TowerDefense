using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PreviewRange : MonoBehaviour
{
    private ITowerUpgradeNotifier notifier;
    private LineRenderer lineRenderer;
    private int segments = 100;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Start()
    {
        notifier = SOManager.Instance as ITowerUpgradeNotifier;
        notifier.OnTowerUpgraded += HandleTowerUpgraded;
    }

    private void OnDestroy()
    {
        notifier.OnTowerUpgraded -= HandleTowerUpgraded;
    }

    private void HandleTowerUpgraded(TowerType towerType)
    {
        BaseTower obj = ClickManager.Instance.nowClickObject as BaseTower;
        AttackStats attackStat = SOManager.Instance.GetTowerRuntimeStat(towerType);

        if (obj == gameObject.GetComponentInParent<BaseTower>())
        {
            Debug.Log("PreviewRange: 타워 업그레이드 감지, 사거리 갱신");
            SetRangePreview(attackStat.range);
        }
    }

    public void SetRangePreview(float range)
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * range;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            angle += 360f / segments;
        }
    }

    public void SetRangeObjectState(float range, bool bFlag)
    {
        gameObject.SetActive(bFlag);
        if (bFlag)
        {
            SetRangePreview(range);
        }
    }
}
