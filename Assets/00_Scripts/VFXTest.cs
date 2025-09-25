using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTest : MonoBehaviour
{
    public VisualEffect vfx;
    public Vector3 startpos;
    public Vector3 endpos;

    void Awake()
    {
        if (vfx == null)
            vfx = GetComponent<VisualEffect>();

        if (startpos != null)
            endpos = new Vector3(startpos.x, startpos.y, startpos.z + 10);
        StartCoroutine(test());
    }
    IEnumerator test()
    {
        while (true)
        {
            vfx.SetVector3("StartPosition", startpos);
            vfx.SetVector3("EndPosition", endpos);
            vfx.SendEvent("OnPlay");
            yield return new WaitForSeconds(1.5f);
        }
    }
}
