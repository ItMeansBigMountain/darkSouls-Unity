using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName ="A.I / ENEMY ACTIONS / ATTACK ACTION")]


    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2; 

        public float maximumAttackAngle = 35; 
        public float minimumAttackAngle = -35; 
        
        public float maximumDistanceNeededToAttack = 3; 
        public float minimumDistanceNeededToAttack = 0; 
   
    }

}


