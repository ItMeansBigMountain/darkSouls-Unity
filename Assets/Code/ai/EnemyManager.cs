using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AF
{

    public class EnemyManager : CharacterManager
    {
        // public EnemyAttackAction[] enemyAttacks;
        // public EnemyAttackAction currentAttack;

        public State currentState;

        enemyLocomotion enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        public NavMeshAgent navMeshAgent;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidBody;


        public float distanceFromTarget;
        public float rotationSpeed = 15;
        public float maximumAttackRange = 1.5f;

        public bool isPreformingAction;




        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float viewableAngle;
        public float currentRecoveryTime = 0;





        private void Awake()
        {
            enemyLocomotionManager = GetComponent<enemyLocomotion>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidBody = GetComponent<Rigidbody>();

            navMeshAgent.enabled = false;
        }

        public void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTime();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }










        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }
            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }

    }

}


