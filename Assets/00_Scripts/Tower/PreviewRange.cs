using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PreviewRange : MonoBehaviour
{
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
