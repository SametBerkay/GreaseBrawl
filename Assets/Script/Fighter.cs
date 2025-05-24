using UnityEngine;

[System.Serializable]
public class Fighter
{
    public string fighterName;
    public float maxHealth = 100f;
    public float damagePerHit = 10f;
    public float odds = 1.5f; // Kazanma oranı (örnek: 1.5x para)

    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isAlive => currentHealth > 0;

    public Wrestler wrestlerInstance; // Sahnedeki fiziki güreşçi objesi

    public void ResetFighter()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }
}
