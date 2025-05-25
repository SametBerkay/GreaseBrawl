using UnityEngine;

public class FighterSelectPanel : MonoBehaviour
{
    public static FighterSelectPanel Instance;

    public FighterSelectUI box1;
    public FighterSelectUI box2;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // ❗ GameManager'daki currentPair üzerinden fighter'ları gösterir
    public void SetupFromGameManager()
    {
        Fighter[] pair = GameManager.Instance.GetCurrentPair();

        box1.Setup(pair[0]);
        box2.Setup(pair[1]);

        box1.SetActive(false);
        box2.SetActive(false);
    }

    // ❗ Oyuncunun seçtiği kutuyu aktif hale getirir
    public void SetActiveFighter(FighterSelectUI selected)
    {
        box1.SetActive(false);
        box2.SetActive(false);
        selected.SetActive(true);
    }
}
