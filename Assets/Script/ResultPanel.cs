using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class ResultPanel : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public TextMeshProUGUI betFighterText;
    public TextMeshProUGUI winnerFighterText;
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI balanceText;
    public Button continueButton;

    private Fighter betFighter;
    private Fighter winner;
    private int betAmount;
    private int earnings;

    public void ShowResult(Fighter selectedFighter, Fighter winnerFighter, int amount)
    {
        betFighter = selectedFighter;
        winner = winnerFighter;
        betAmount = amount;

        bool playerWon = (winner == betFighter);
        earnings = playerWon ? Mathf.RoundToInt(betAmount * betFighter.odds) : 0;

        if (earnings > 0)
        {
            PlayerData.Instance.AddMoney(earnings);
        }

        betFighterText.text = $"Bahis Yapılan: {betFighter.fighterName}";
        winnerFighterText.text = $"Kazanan: {(winner != null ? winner.fighterName : "Beraberlik")}";
        earningsText.text = $"Kazanç: ${earnings}";
        balanceText.text = $"Güncel Bakiye: ${PlayerData.Instance.money}";

        gameObject.SetActive(true);

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        if (PlayerData.Instance.money >= 50)
        {
            gameObject.SetActive(false);
            GameManager.Instance.StartCoroutine(GameManager.Instance.NextFightDelay());
        }
        else
        {
            SceneManager.LoadScene("DefeatScene");
        }
    }
}
