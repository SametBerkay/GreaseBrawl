using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panelleri")]
    public GameObject fighterIntroPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI playerMoneyText;

    [Header("Bahis Sistemi")]
    public Fighter selectedFighter;
    public int betAmount = 0;

    [Header("Fighter Havuzu")]
    public Fighter[] fighters; // 2 adet fighter inspector'dan atanacak

    [Header("Dövüş")]
    public FightManager fightManager;

    [Header("Result Panel")]
    public ResultPanel resultPanel; // 👈 Yeni result panel

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
        gameOverPanel.SetActive(false);

        FighterSelectPanel.Instance.Setup(fighters);
        UpdateMoneyUI();
    }

    public void SelectFighter(Fighter fighter, int amount)
    {
        if (!PlayerData.Instance.TrySpend(amount))
        {
            Debug.LogWarning("Yetersiz bakiye!");
            return;
        }

        selectedFighter = fighter;
        betAmount = amount;

        UpdateMoneyUI();

        Fighter enemy = (fighters[0] == selectedFighter) ? fighters[1] : fighters[0];
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
    Debug.Log("✅ OnFightFinished çağrıldı.");

    if (resultPanel == null)
    {
        Debug.LogError("❌ ResultPanel atanmadı!");
        return;
    }

    resultPanel.ShowResult(selectedFighter, winner, betAmount);
    UpdateMoneyUI();
}


    public IEnumerator NextFightDelay()
    {
        yield return new WaitForSeconds(2f);
        ShowFighterIntro();
    }

    private void UpdateMoneyUI()
    {
        playerMoneyText.text = $"${PlayerData.Instance.money}";
    }
}
