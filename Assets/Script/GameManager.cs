using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Fighter Prefab Havuzu")]
    public Fighter[] allFighterPrefabs;

    [Header("UI Panelleri")]
    public GameObject fighterIntroPanel;
    public GameObject resultPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI playerMoneyText;

    [Header("Oyun Kuralları")]
    public int currentMatch = 1;
    public int maxMatches = 5;
    public int winTarget = 2000;

    [Header("Dövüş")]
    public FightManager fightManager;
    public Transform spawnPointA;
    public Transform spawnPointB;

    private Fighter[] currentPair;
    private Fighter selectedFighter;
    private int betAmount;

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
        GenerateRandomFighterPair();

        fighterIntroPanel.SetActive(true);
        resultPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        FighterSelectPanel.Instance.SetupFromGameManager();
        UpdateMoneyUI();
    }

    private void GenerateRandomFighterPair()
    {
        Fighter f1 = allFighterPrefabs[Random.Range(0, allFighterPrefabs.Length)];
        Fighter f2;
        do
        {
            f2 = allFighterPrefabs[Random.Range(0, allFighterPrefabs.Length)];
        } while (f2 == f1);

        currentPair = new Fighter[] { f1, f2 };
    }

    public Fighter[] GetCurrentPair() => currentPair;

    public void SelectFighter(Fighter fighterData, int amount)
    {
        if (!PlayerData.Instance.TrySpend(amount)) return;

        betAmount = amount;

        Fighter enemyData = (currentPair[0] == fighterData) ? currentPair[1] : currentPair[0];

        GameObject aObj = Instantiate(fighterData.wrestlerPrefab, spawnPointA.position, Quaternion.identity);
        GameObject bObj = Instantiate(enemyData.wrestlerPrefab, spawnPointB.position, Quaternion.identity);

        Wrestler wa = aObj.GetComponent<Wrestler>();
        Wrestler wb = bObj.GetComponent<Wrestler>();

        selectedFighter = new Fighter
        {
            fighterName = fighterData.fighterName,
            maxHealth = fighterData.maxHealth,
            damagePerHit = fighterData.damagePerHit,
            odds = fighterData.odds,
            portrait = fighterData.portrait,
            wrestlerPrefab = fighterData.wrestlerPrefab,
            wrestlerInstance = wa
        };

        Fighter enemyFighter = new Fighter
        {
            fighterName = enemyData.fighterName,
            maxHealth = enemyData.maxHealth,
            damagePerHit = enemyData.damagePerHit,
            odds = enemyData.odds,
            portrait = enemyData.portrait,
            wrestlerPrefab = enemyData.wrestlerPrefab,
            wrestlerInstance = wb
        };

        wa.fighterData = selectedFighter;
        wb.fighterData = enemyFighter;

        fightManager.SetFighters(selectedFighter, enemyFighter);
        UpdateMoneyUI();
        fighterIntroPanel.SetActive(false);
        StartCoroutine(StartFightSequence());
    }

    private IEnumerator StartFightSequence()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3"; yield return new WaitForSeconds(1);
        countdownText.text = "2"; yield return new WaitForSeconds(1);
        countdownText.text = "1"; yield return new WaitForSeconds(1);
        countdownText.text = "Fight!"; yield return new WaitForSeconds(0.5f);
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
            PlayerData.Instance.AddMoney(earnings);
        }

        UpdateMoneyUI();

        if (PlayerData.Instance.money >= winTarget)
        {
            SceneManager.LoadScene("WinScene");
            return;
        }

        if (PlayerData.Instance.money <= 0 && !playerWon)
        {
            gameOverPanel.SetActive(true);
            return;
        }

        currentMatch++;
        if (currentMatch > maxMatches)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(NextFightDelay());
        }
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