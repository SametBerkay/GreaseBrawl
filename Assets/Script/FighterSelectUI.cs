using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FighterSelectUI : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI oddsText;

    public Button selectButton;
    public Button startButton;
    public GameObject betPanel; // içinde: plus, minus, betText

    public BetController betController;

    private Fighter fighterData;

    public void Setup(Fighter fighter)
    {
        fighterData = fighter;

        nameText.text = fighter.fighterName;
        healthText.text = $"Can: {fighter.maxHealth}";
        damageText.text = $"Hasar: {fighter.damagePerHit}";
        oddsText.text = $"Oran: {fighter.odds}x";

        betPanel.SetActive(false);
        startButton.gameObject.SetActive(false);
        betController.ResetBet();

        selectButton.onClick.AddListener(OnFighterSelected);
        startButton.onClick.AddListener(OnStartClicked);
    }

private void OnFighterSelected()
{
    Debug.Log($"Seçildi: {fighterData.fighterName}");
    FighterSelectPanel.Instance.SetActiveFighter(this);
}

    public void SetActive(bool state)
    {
        betPanel.SetActive(state);
        startButton.gameObject.SetActive(state);
    }

    private void OnStartClicked()
    {
        int bet = betController.CurrentBet;
        GameManager.Instance.SelectFighter(fighterData, bet);
        FighterSelectPanel.Instance.gameObject.SetActive(false);
    }
}
