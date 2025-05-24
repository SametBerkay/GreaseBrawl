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

    [Header("Butonlar ve Panel")]
    public Button selectButton;
    public Button startButton;
    public GameObject betPanel;

    [Header("Bet Controller")]
    public BetController betController;

    private Fighter fighterData;
    private bool isSelected = false;

    public void Setup(Fighter fighter)
    {
        fighterData = fighter;

        nameText.text = fighter.fighterName;
        healthText.text = $"Can: {fighter.maxHealth}";
        damageText.text = $"Hasar: {fighter.damagePerHit}";
        oddsText.text = $"Oran: {fighter.odds}x";

        SetSelected(false);
        SetActiveUI(false);
        betController.ResetBet();

        selectButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();

        selectButton.onClick.AddListener(OnFighterSelected);
        startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnFighterSelected()
    {
        Debug.Log($"🔘 Seçildi: {fighterData.fighterName}");
        FighterSelectPanel.Instance.SetActiveFighter(this);
    }

    private void OnStartClicked()
    {
        int bet = betController.CurrentBet;
        Debug.Log($"🎯 Start basıldı → {fighterData.fighterName} için ₺{bet} bahis");

        GameManager.Instance.SelectFighter(fighterData, bet);
        FighterSelectPanel.Instance.gameObject.SetActive(false);
    }

    public void SetActiveUI(bool state)
    {
        betPanel.SetActive(state);
        startButton.gameObject.SetActive(state);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        SetActiveUI(selected);
    }

    public bool IsSelected => isSelected;
}
