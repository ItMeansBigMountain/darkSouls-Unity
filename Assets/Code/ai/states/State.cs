using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager , EnemyStats enemyStats , EnemyAnimatorManager enemyAnimatorManager);



    }
}


