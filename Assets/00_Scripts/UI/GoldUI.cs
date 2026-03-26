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
        PlayerManager.Instance.OnGoldChanged += UpdateGoldText;

        UpdateGoldText(PlayerManager.Instance.Gold);
    }

    void OnDisable()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnGoldChanged -= UpdateGoldText;
    }

    private void UpdateGoldText(int currentGold)
    {
        if (goldText == null) return;

        goldText.text = ($"{currentGold}G");
    }
}
