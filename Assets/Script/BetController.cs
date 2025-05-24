using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BetController : MonoBehaviour
{
    public Button plusButton;
    public Button minusButton;
    public TextMeshProUGUI betAmountText;

    private int currentBet = 50;

    public int CurrentBet => currentBet;

    private void Start()
    {
        UpdateUI();

        plusButton.onClick.AddListener(() =>
        {
            currentBet += 50;
            UpdateUI();
        });

        minusButton.onClick.AddListener(() =>
        {
            if (currentBet > 50)
                currentBet -= 50;

            UpdateUI();
        });
    }

    private void UpdateUI()
    {
        betAmountText.text = $"Bahis: ₺{currentBet}";
    }

    public void ResetBet()
    {
        currentBet = 50;
        UpdateUI();
    }
}
