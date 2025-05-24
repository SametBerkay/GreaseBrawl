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
    public float fightSpeedMultiplier = 1f;

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
        isFightActive = true;
        fightTimer = fightDuration;

        fighterA.ResetFighter();
        fighterB.ResetFighter();

        fighterA.wrestlerInstance.SetAnimSpeed(fightSpeedMultiplier);
        fighterB.wrestlerInstance.SetAnimSpeed(fightSpeedMultiplier);

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
        yield return attacker.wrestlerInstance.MoveTo(defender.wrestlerInstance.transform.position - Vector3.right * 1f, fightSpeedMultiplier);
        yield return defender.wrestlerInstance.MoveTo(attacker.wrestlerInstance.transform.position + Vector3.right * 1f, fightSpeedMultiplier);

        attacker.wrestlerInstance.PlayAnimation("Attack");
        defender.wrestlerInstance.PlayAnimation("TakeDamage");

        defender.TakeDamage(attacker.damagePerHit);

        yield return new WaitForSeconds(0.5f / fightSpeedMultiplier);

        yield return attacker.wrestlerInstance.MoveTo(attacker.wrestlerInstance.originalPosition, fightSpeedMultiplier);
        yield return defender.wrestlerInstance.MoveTo(defender.wrestlerInstance.originalPosition, fightSpeedMultiplier);
    }

    private IEnumerator DoGrapplePhase()
    {
        Vector3 center = (fighterA.wrestlerInstance.originalPosition + fighterB.wrestlerInstance.originalPosition) / 2f;

        yield return fighterA.wrestlerInstance.MoveTo(center + Vector3.left * 0.5f, fightSpeedMultiplier);
        yield return fighterB.wrestlerInstance.MoveTo(center + Vector3.right * 0.5f, fightSpeedMultiplier);

        fighterA.wrestlerInstance.PlayAnimation("IsFight");
        fighterB.wrestlerInstance.PlayAnimation("IsFight");

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
        isFightActive = false;

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
