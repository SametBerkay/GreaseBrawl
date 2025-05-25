using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FighterSelectUI : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public Image portrait;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI oddsText;

    [Header("Butonlar ve Bahis Paneli")]
    public Button selectButton;
    public Button startButton;
    public GameObject betPanel;
    public BetController betController;

    private Fighter fighterData;

    public void Setup(Fighter fighter)
    {
        fighterData = fighter;

        nameText.text = fighter.fighterName;
        healthText.text = $"Can: {fighter.maxHealth}";
        damageText.text = $"Hasar: {fighter.damagePerHit}";
        oddsText.text = $"Oran: {fighter.odds}x";
        portrait.sprite = fighter.portrait;

        SetActive(false);
        betController.ResetBet();

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnFighterSelected);

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnFighterSelected()
    {
        FighterSelectPanel.Instance.SetActiveFighter(this);
    }

    private void OnStartClicked()
    {
        int bet = betController.CurrentBet;
        GameManager.Instance.SelectFighter(fighterData, bet);
        FighterSelectPanel.Instance.gameObject.SetActive(false);
    }

    public void SetActive(bool state)
    {
        betPanel.SetActive(state);
        startButton.gameObject.SetActive(state);
    }
}
