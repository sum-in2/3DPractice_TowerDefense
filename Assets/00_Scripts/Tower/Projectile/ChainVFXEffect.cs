using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.VFX;

public class ChainVFXEffect : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private float autoReturnDelay = 2f;
    Coroutine returnCoroutine;
    void Awake()
    {
        if (vfx == null)
            vfx = GetComponent<VisualEffect>();
    }

    public void PlayEffect(Vector3 startPos, Vector3 endPos, float lifeTime = 1f)
    {
        if (vfx == null) return;
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        vfx.SetVector3("StartPos", startPos);
        vfx.SetVector3("EndPos", endPos);

        vfx.Play();

        returnCoroutine = StartCoroutine(AutoReturn());
    }

    IEnumerator AutoReturn()
    {
        yield return new WaitForSeconds(autoReturnDelay);
        ObjectPoolManager.Instance.ReturnObject(this);
        returnCoroutine = null;
    }

    void OnEnable()
    {
        if (vfx != null)
        {
            vfx.Stop();
            vfx.Reinit();
        }
    }

    void OnDisable()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }

        if (vfx != null)
            vfx.Stop();
    }
}
