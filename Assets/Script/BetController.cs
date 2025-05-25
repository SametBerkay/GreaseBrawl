
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BetController : MonoBehaviour
{
    public Button plusButton;
    public Button minusButton;
    public TextMeshProUGUI betAmountText;
    public TextMeshProUGUI balanceText; // opsiyonel

    private int currentBet = 50;
    public int CurrentBet => currentBet;

    private void Start()
    {
        plusButton.onClick.AddListener(IncreaseBet);
        minusButton.onClick.AddListener(DecreaseBet);
        UpdateUI();
    }

    private void IncreaseBet()
    {
        if (currentBet + 50 <= PlayerData.Instance.money)
        {
            currentBet += 50;
            UpdateUI();
        }
    }

    private void DecreaseBet()
    {
        if (currentBet > 50)
        {
            currentBet -= 50;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        betAmountText.text = $"Bahis: ${currentBet}";
        if (balanceText != null)
            balanceText.text = $"Bakiye: ${PlayerData.Instance.money}";
    }

    public void ResetBet()
    {
        currentBet = 50;
        UpdateUI();
    }
}
