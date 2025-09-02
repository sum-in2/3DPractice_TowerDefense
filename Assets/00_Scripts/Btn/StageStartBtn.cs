using UnityEngine;

public class StageStartBtn : MonoBehaviour
{
    public int stageLevel = 1;

    public void OnClickStageBtn()
    {
        GameManager.Instance.StartStage(stageLevel);
    }
}
