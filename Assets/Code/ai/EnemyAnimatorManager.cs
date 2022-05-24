using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AF
{
    public class EnemyAnimatorManager : AnimationManager
    {

        EnemyManager enemyManager;


        public void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }


        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;
        }

    }
}


