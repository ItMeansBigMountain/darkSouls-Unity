using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class EnemyManager : CharacterManager
    {
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;



        enemyLocomotion enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        public bool isPreformingAction;


        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;


        public float currentRecoveryTime = 0;




        private void Awake()
        {
            enemyLocomotionManager = GetComponent<enemyLocomotion>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }





        private void Update()
        {
            HandleRecoveryTime();
        }


        private void FixedUpdate()
        {
            HandleCurrentAction();
        }




        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.currentTarget != null)
            {
                enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
            }

            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.handleDetection();
            }
            else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
            else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
            {
                enemyLocomotionManager.navMeshAgent.enabled = false;
                attackTarget();
            }
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


        private void GetNewAttack()
        {
            Vector3 targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null) return; // check if already attacking

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }


        }



        private void attackTarget()
        {
            if (isPreformingAction) return;


            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPreformingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }

        }




    }

}


