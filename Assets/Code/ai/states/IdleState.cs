using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public class IdleState : State
    {
        public pursueTargetState pursueState;
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            # region Handle Enemy Target Detection

            // create sphere over enemy
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);


            // iteration of all things hit
            for (int i = 0; i < colliders.Length; i++)
            {
                // objects hit with stats script attached
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                // if object has stats
                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    // if within field of view
                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle switch state
            if (enemyManager.currentTarget != null)
            {
                return pursueState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }

}

