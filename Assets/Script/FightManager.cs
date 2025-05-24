using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    private Fighter fighterA;
    private Fighter fighterB;

    public TextMeshProUGUI fightTimerText;

    public float fightDuration = 60f;
    public float attackInterval = 5f;
    public float fightSpeedMultiplier = 0.5f;

    private float fightTimer = 0f;
    private bool isFightActive = false;

    public Action<Fighter> onFightFinishedCallback;

    public void SetFighters(Fighter a, Fighter b)
    {
        fighterA = a;
        fighterB = b;
    }

    private void Update()
    {
        if (!isFightActive) return;

        if (fightTimer > 0f)
        {
            fightTimer -= Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void BeginFight(Action<Fighter> onFinished)
    {
        onFightFinishedCallback = onFinished;
        StartCoroutine(FightLoop());
    }

    private IEnumerator FightLoop()
    {
        Debug.Log($"Fight başlıyor → A: {fighterA?.fighterName}, B: {fighterB?.fighterName}");
        Debug.Log($"A wrestlerInstance: {fighterA.wrestlerInstance}, B: {fighterB.wrestlerInstance}");

        isFightActive = true;
        fightTimer = fightDuration;
        if (fightTimerText != null) fightTimerText.gameObject.SetActive(true);

        fighterA.ResetFighter();
        fighterB.ResetFighter();

        while (fightTimer > 0 && fighterA.isAlive && fighterB.isAlive)
        {
            yield return StartCoroutine(DoAttackPhase(fighterA, fighterB));
            if (!fighterB.isAlive) break;

            yield return StartCoroutine(DoAttackPhase(fighterB, fighterA));
            if (!fighterA.isAlive) break;

            yield return StartCoroutine(DoGrapplePhase());

            fightTimer -= attackInterval;
        }

        EndFight();
    }

    private IEnumerator DoAttackPhase(Fighter attacker, Fighter defender)
    {
        Vector3 attackerStart = attacker.wrestlerInstance.originalPosition;
        Vector3 defenderStart = defender.wrestlerInstance.originalPosition;
        Vector3 approachPoint = defenderStart + Vector3.left * 0.5f;

        yield return attacker.wrestlerInstance.MoveTo(approachPoint, fightSpeedMultiplier);

        defender.TakeDamage(attacker.damagePerHit);

        yield return new WaitForSeconds(0.2f / fightSpeedMultiplier);

        yield return attacker.wrestlerInstance.MoveTo(attackerStart, fightSpeedMultiplier);
    }

    private IEnumerator DoGrapplePhase()
    {
        Vector3 center = (fighterA.wrestlerInstance.originalPosition + fighterB.wrestlerInstance.originalPosition) / 2f;

        yield return fighterA.wrestlerInstance.MoveTo(center + Vector3.left * 0.5f, fightSpeedMultiplier);
        yield return fighterB.wrestlerInstance.MoveTo(center + Vector3.right * 0.5f, fightSpeedMultiplier);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.3f / fightSpeedMultiplier);
            Vector3 temp = fighterA.wrestlerInstance.transform.position;
            fighterA.wrestlerInstance.transform.position = fighterB.wrestlerInstance.transform.position;
            fighterB.wrestlerInstance.transform.position = temp;
        }

        yield return fighterA.wrestlerInstance.MoveTo(fighterA.wrestlerInstance.originalPosition, fightSpeedMultiplier);
        yield return fighterB.wrestlerInstance.MoveTo(fighterB.wrestlerInstance.originalPosition, fightSpeedMultiplier);
    }

    private void EndFight()
    {
        Debug.Log("💀 EndFight() çağrıldı.");

        isFightActive = false;
        if (fightTimerText != null)
        {
            fightTimerText.gameObject.SetActive(false); // 👈 Timer artık gizleniyor
        }

        Fighter winner = null;

        if (fighterA.isAlive && !fighterB.isAlive) winner = fighterA;
        else if (!fighterA.isAlive && fighterB.isAlive) winner = fighterB;

        onFightFinishedCallback?.Invoke(winner);
    }

    private void UpdateTimerUI()
    {
        if (fightTimerText == null) return;
        int minutes = Mathf.FloorToInt(fightTimer / 60f);
        int seconds = Mathf.FloorToInt(fightTimer % 60f);
        fightTimerText.text = $"{minutes:00}:{seconds:00}";
    }
}
