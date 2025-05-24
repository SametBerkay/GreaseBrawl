using UnityEngine;

[System.Serializable]
public class Fighter
{
    public string fighterName;
    public float maxHealth = 100f;
    public float damagePerHit = 10f;
    public float odds = 1.5f;
    public Wrestler wrestlerInstance;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isAlive => currentHealth > 0;

   public void ResetFighter()
{
    currentHealth = maxHealth;

    if (wrestlerInstance != null)
        wrestlerInstance.SetMaxHealth(maxHealth);
}


   public void TakeDamage(float amount)
{
    currentHealth -= amount;
    if (currentHealth < 0) currentHealth = 0;

    if (wrestlerInstance != null)
        wrestlerInstance.TakeDamage(amount);
}

} 