using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    protected Canvas canvas;

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        Init();
    }

    protected abstract void Init();

    public virtual void Show()
    {
        gameObject.SetActive(true);
        if (canvas != null)
            canvas.enabled = true;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        if (canvas != null)
            canvas.enabled = false;
    }

    public void SetSortingOrder(int order)
    {
        if (canvas != null)
            canvas.sortingOrder = order;
    }
}
