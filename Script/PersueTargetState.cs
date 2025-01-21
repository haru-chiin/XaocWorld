using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersueTargetState : State
{
    public CombatStanceState combatStanceState;
    public RotateTowardState rotateTowardState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);
        HandleRotateTowardsTarget(enemyManager);

        if (enemyManager.isInteracting)
            return this;

        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 01f, Time.deltaTime);
            return this;
        }
  

        if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
        


        /*transform.position = new Vector3(transform.position.x, navmeshAgent.transform.position.y, transform.position.z);*/
/*        enemyManager.navmeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navmeshAgent.transform.localRotation = Quaternion.identity;*/

        if(distanceFromTarget <= enemyManager.maximumAggroRadius)
        {
            return combatStanceState;
        }
        return this;
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        if (enemyManager.isPerformingAction)
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
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

            enemyManager.navmeshAgent.enabled = true;
            enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }

        /* navmeshAgent.transform.localPosition = Vector3.zero;
         navmeshAgent.transform.localRotation = Quaternion.identity;*/
    }
}
