using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panelleri")]
    public GameObject fighterIntroPanel;
    public GameObject resultPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI playerMoneyText;

    [Header("Bahis Sistemi")]
    public int playerMoney = 100;
    public Fighter selectedFighter;
    public int betAmount = 0;

    [Header("Fighter Havuzu")]
    public Fighter[] fighters; // 2 adet fighter inspector'dan atanacak

    [Header("Dövüş")]
    public FightManager fightManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowFighterIntro();
    }

  public void ShowFighterIntro()
{
    
    fighterIntroPanel.SetActive(true);
    resultPanel.SetActive(false);
    gameOverPanel.SetActive(false);

    FighterSelectPanel.Instance.Setup(fighters); // 🟢 BU SATIR

    UpdateMoneyUI();
}


    public void SelectFighter(Fighter fighter, int amount)
    {
        if (amount > playerMoney) return;

        selectedFighter = fighter;
        betAmount = amount;
        playerMoney -= amount;
        UpdateMoneyUI();

        // Rakip olan fighter'ı seç
        Fighter enemy = (fighters[0] == selectedFighter) ? fighters[1] : fighters[0];

        // Fighter’ları FightManager’a gönder
        fightManager.SetFighters(selectedFighter, enemy);

        fighterIntroPanel.SetActive(false);
        StartCoroutine(StartFightSequence());
    }

    private IEnumerator StartFightSequence()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "Fight!";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);

        fightManager.BeginFight(OnFightFinished);
    }

    private void OnFightFinished(Fighter winner)
    {
        resultPanel.SetActive(true);
        bool playerWon = (winner == selectedFighter);

        if (playerWon)
        {
            int earnings = Mathf.RoundToInt(betAmount * selectedFighter.odds);
            playerMoney += earnings;
            // Kazanma sonucu UI’ya yazdırılabilir
        }

        UpdateMoneyUI();

        if (playerMoney <= 0 && !playerWon)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(NextFightDelay());
        }
    }

    private IEnumerator NextFightDelay()
    {
        yield return new WaitForSeconds(2f);
        ShowFighterIntro();
    }

    private void UpdateMoneyUI()
    {
        playerMoneyText.text = $"₺{playerMoney}";
    }
}
