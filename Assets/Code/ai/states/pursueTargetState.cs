using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public class pursueTargetState : State
    {

        public CombatStanceState combatStanceState;


        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPreformingAction) return this; // checking if preforming an action

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                print(enemyManager.viewableAngle);
            }
            // else if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            // {
            //     enemyManager.navMeshAgent.enabled = false;
            //     enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            //     enemyManager.enemyRigidBody.velocity = Vector3.zero;
            //     print(enemyManager.maximumAttackRange);
            // }

            HandleRotationTowardsTarget(enemyManager);
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;
            if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }

        }

        private void HandleRotationTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPreformingAction)
            {
                //rotate manually
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
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;
                // Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity; // VIDEO 31
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidBody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(
                    enemyManager.transform.rotation,
                    enemyManager.navMeshAgent.transform.rotation,
                    enemyManager.rotationSpeed / Time.deltaTime
                );
            }

        }

    }
}


