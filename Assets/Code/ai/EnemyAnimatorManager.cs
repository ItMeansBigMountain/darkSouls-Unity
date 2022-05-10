using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AF
{
    public class EnemyAnimatorManager : AnimationManager
    {

        enemyLocomotion enemyLocomotionManager;


        public void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotionManager = GetComponentInParent<enemyLocomotion>();
        }


        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyLocomotionManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyLocomotionManager.enemyRigidBody.velocity = velocity;
        }

    }
}


