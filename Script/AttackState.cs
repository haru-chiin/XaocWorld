using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public RotateTowardState rotateTowardState;
    public CombatStanceState combatStanceState;
    public PersueTargetState puruseTargetState;
    public EnemyAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;


    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        RotateTowardsTargetWhilsAttacking(enemyManager);

        if(distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            return puruseTargetState;
        }

        if(willDoComboOnNextAttack && enemyManager.canDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
        }

        if (!hasPerformedAttack)
        {
            AttackTarget(enemyAnimatorManager, enemyManager);
            RollForComboChance(enemyManager);
        }

        if(willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this;
        }

        return rotateTowardState;
    }

    private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        enemyAnimatorManager.PlayWeaponTrailFX();
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        willDoComboOnNextAttack = false;
        enemyAnimatorManager.PlayWeaponTrailFX();
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilsAttacking(EnemyManager enemyManager)
    {
        if (enemyManager.canRotate && enemyManager.isInteracting)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }

        /* navmeshAgent.transform.localPosition = Vector3.zero;
         navmeshAgent.transform.localRotation = Quaternion.identity;*/
    }

    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
        {
            if(currentAttack.comboAction != null)
            {
                willDoComboOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }

            
        }
    }
}
