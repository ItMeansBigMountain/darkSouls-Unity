using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public class EnemyStats : CharacterStats
    {
        
            Animator animator;



            private void Awake()
            {
                animator = GetComponentInChildren<Animator>();
            }

            void Start()
            {
                    maxHealth = SetMaxHealthFromHealthLevel();
                    CurrentHealth = maxHealth;
            }

            private int SetMaxHealthFromHealthLevel()
            {
                maxHealth = healthLevel * 10;
                return maxHealth;
            }

            public void TakeDamage(int damage)
            {
                CurrentHealth = CurrentHealth - damage;

                animator.Play("Damage_01");

                if(CurrentHealth <= 0)
                {
                    CurrentHealth = 0;
                    animator.Play("Death_01");
                    //HANDLE ENEMY DEATH
                }

            }


    }

}
