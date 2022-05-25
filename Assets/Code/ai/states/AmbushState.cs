using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius;
        public LayerMask detectionLayer;

        public pursueTargetState pursueTargetState;

        public string sleepAnimation;
        public string wakeAnimation;






        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (isSleeping && enemyManager.isInteracting == false)
            {
                enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }


            #region Handle Target Detection

            //create sphere to check for playerManagers within range of detection radius
            Collider[] colliders = Physics.OverlapSphere(
                enemyManager.transform.position, detectionRadius, detectionLayer
            );

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    //subtraction of vectors is the direction
                    Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                    if ( // check if infront of enemy or gets hit
                        viewableAngle > enemyManager.minimumDetectionAngle
                        && viewableAngle < enemyManager.maximumDetectionAngle
                        || enemyStats.CurrentHealth < enemyStats.maxHealth
                    )
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }
            #endregion

            #region State Change
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion

        }
    }

}
