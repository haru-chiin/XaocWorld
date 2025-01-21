using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidbody;

    public bool isPerformingAction;
    public bool isInteracting;

    public State currentState;
    public characterStats currentTarget;


    public float rotationSpeed = 15;
    public float maximumAggroRadius = 1.5f;

    [Header("Combat Flags")]
    public bool canDoCombo;

    [Header("A.I settings")]
    public float detectionRadius = 20; 
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;

    public float currentRecoveryTime = 0;

    [Header("A.I ComboSettings")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        /*backStabCollider = GetComponentInChildren<CriticalDamageCollider>();*/

        navmeshAgent.enabled = false;
    }
    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
        isPhaseShifting = enemyAnimatorManager.anim.GetBool("isPhaseShifting");
        canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
        enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
    }


    private void LateUpdate()
    {
        
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
        if(currentState != null)
        {
            State nexState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if(nexState != null)
            {
                SwitchToNextState(nexState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    

}
