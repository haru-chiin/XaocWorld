using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public string bossName;

    UIBossHealthBar bossHealthBar;
    EnemyStats enemyStats;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;
    WorldEventManager worldEventManager;

    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHEalth(enemyStats.MaxHealth);
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);


        if (currentHealth < maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
        {
            bossCombatStanceState.hasPhaseShifted = true;
            ShiftToSecondPhase();
        }
        else if(currentHealth <= 0)
        {
            worldEventManager.BossHasBeenDefeated();
            enemyStats.HandleDeath();
        }
    }

    public void ShiftToSecondPhase()
    {
        enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        bossCombatStanceState.hasPhaseShifted = true;
    }

}
