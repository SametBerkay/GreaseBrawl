using UnityEngine;
using DG.Tweening;
using System.Collections;
public class Wrestler : MonoBehaviour
{
    public Animator animator;
    public float health = 100f;
    public float damagePerHit = 10f;
    public Vector3 originalPosition;

    public bool IsAlive => health > 0;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            PlayAnimation("Defeat");
        }
    }

    public void PlayAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void SetAnimSpeed(float speed)
    {
        animator.speed = speed;
    }

    public IEnumerator MoveTo(Vector3 targetPos, float speedMultiplier)
    {
        float duration = 0.3f / speedMultiplier;
        yield return transform.DOMove(targetPos, duration).SetEase(Ease.InOutSine).WaitForCompletion();
    }
}
