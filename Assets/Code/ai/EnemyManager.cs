using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AF
{

    public class EnemyManager : CharacterManager
    {
        public State currentState;

        enemyLocomotion enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        public NavMeshAgent navMeshAgent;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidBody;


        public float rotationSpeed = 50;
        public float maximumAttackRange = 1.5f;

        public bool isPreformingAction;
        public bool isInteracting;




        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
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
            isInteracting = enemyAnimatorManager.anim.GetBool("IsInteracting");
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


