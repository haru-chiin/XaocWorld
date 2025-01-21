using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;

    public LayerMask detectionLayer;

    public PersueTargetState persueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if(isSleeping && enemyManager.isInteracting == false)
        {
            enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            characterStats characterStats = colliders[i].transform.GetComponent<characterStats>();

            if(characterStats != null)
            {
                Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                    isSleeping = false;
                    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }

        #endregion

        #region Handle state Change

        if(enemyManager.currentTarget != null)
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
