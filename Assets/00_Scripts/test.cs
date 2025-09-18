using UnityEngine;
using UnityEngine.VFX;

public class test : MonoBehaviour
{

    public VisualEffect VFXtest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (VFXtest != null)
        {
            VFXEventAttribute eventAttribute = VFXtest.CreateVFXEventAttribute();

            eventAttribute.SetVector3("StartPosition", new Vector3(3, 1, 3));
            eventAttribute.SetVector3("EndPosition", new Vector3(-3, 1, 3));

            VFXtest.SendEvent("OnPlay", eventAttribute);
            Debug.Log("onplay");
        }
    }
}
