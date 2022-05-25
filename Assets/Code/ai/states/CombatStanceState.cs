using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public class CombatStanceState : State
    {

        public AttackState attackState;
        public pursueTargetState pursueState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);


            if (enemyManager.isPreformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            if (enemyManager.currentRecoveryTime <= 0 &&
             distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueState;
            }
            else
            {
                return this;
            }

        }
    }

}


