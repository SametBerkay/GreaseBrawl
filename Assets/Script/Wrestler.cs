 using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Wrestler : MonoBehaviour
{
    public Animator animator;
    public float health = 100f;
    public float damagePerHit = 10f;
    public Vector3 originalPosition;

    public Slider healthSlider;
    public TextMeshProUGUI healthText;
public Fighter fighterData; // 👈 Bu alan yoksa ekle

    public bool IsAlive => health > 0;

    private void Start()
    {
        originalPosition = transform.position;
        UpdateHealthUI(); // başlangıçta slider ve text doldur
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;

        UpdateHealthUI();

        if (health <= 0)
        {
            PlayAnimation("Defeat");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        if (healthText != null)
        {
            healthText.text = $"Can: {Mathf.RoundToInt(health)}";
        }
    }

    public void PlayAnimation(string triggerName)
    {
        if (animator != null)
            animator.SetTrigger(triggerName);
    }

    public void SetAnimSpeed(float speed)
    {
        if (animator != null)
            animator.speed = speed;
    }

    public IEnumerator MoveTo(Vector3 targetPos, float speedMultiplier)
    {
        float duration = 0.5f / speedMultiplier; // 🔄 daha yavaş dövüş
        yield return transform.DOMove(targetPos, duration).SetEase(Ease.InOutSine).WaitForCompletion();
    }

    public void SetMaxHealth(float maxHealth)
    {
        health = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        UpdateHealthUI();
    }
} 