using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int money = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool TrySpend(int amount)
    {
        if (amount > money) return false;

        money -= amount;
        return true;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void ResetMoney(int value)
    {
        money = value;
    }
}
