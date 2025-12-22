using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Awake()
    {
        if (goldText == null)
            goldText = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        GameManager.Instance.OnGoldChanged += UpdateGoldText;

        UpdateGoldText(GameManager.Instance.Gold);
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGoldChanged -= UpdateGoldText;
    }

    private void UpdateGoldText(int currentGold)
    {
        if (goldText == null) return;

        goldText.text = ($"{currentGold}G");
    }
}
