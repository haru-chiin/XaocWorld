using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PersueTargetState persueTargetState;
    public LayerMask detectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        #region Handle Enemy Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                characterStats characterStats = colliders[i].transform.GetComponent<characterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
        #endregion

        #region handle Switching to Next State
        if (enemyManager.currentTarget != null)
        {
            return persueTargetState;
        }
        else
        {
            return this;
        }
        #endregion
    }
}
