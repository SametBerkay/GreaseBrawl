using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        // Kazanma durumu ve kazanç hesapla
        bool playerWon = (winner == betFighter);
        earnings = playerWon ? Mathf.RoundToInt(betAmount * betFighter.odds) : 0;

        // Parayı ekle
        if (earnings > 0)
        {
            PlayerData.Instance.AddMoney(earnings);
        }

        // UI'ya yaz
        betFighterText.text = $"Bahis Yapılan: {betFighter.fighterName}";
        winnerFighterText.text = $"Kazanan: {(winner != null ? winner.fighterName : "Beraberlik")}";
        earningsText.text = $"Kazanç: ${earnings}";
        balanceText.text = $"Güncel Bakiye: ${PlayerData.Instance.money}";

        gameObject.SetActive(true);

        // Continue butonuna tıklama
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        int balance = PlayerData.Instance.money;

        // 🟢 Kazanma kontrolü
        if (balance >= GameManager.Instance.winTarget)
        {
            SceneManager.LoadScene("WinScene");
        }
        // 🔴 Kaybetme kontrolü (para yetmez veya maç limiti dolmuş)
        else if (balance < 50 || GameManager.Instance.currentMatch >= GameManager.Instance.maxMatches)
        {
            SceneManager.LoadScene("DefeatScene");
        }
        // 🔁 Devam edilebilir
        else
        {
            gameObject.SetActive(false);
            GameManager.Instance.StartCoroutine(GameManager.Instance.NextFightDelay());
        }
    }
}
