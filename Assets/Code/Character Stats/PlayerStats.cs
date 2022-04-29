using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class PlayerStats : MonoBehaviour
    {

        public int healthLevel = 10;
        public int maxHealth;
        public int CurrentHealth;

        public int staminaLevel = 5;
        public float currentStamina;
        public int maxStamina;


        public HealthBar healthBar;
        public StaminaBar staminaBar;

        AnimateHandler animatorHandler;


        // INIT
        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimateHandler>();
        }
        void Start()
        {
                // setting hp
                maxHealth = SetMaxHealth_FromHealthLevel();
                CurrentHealth = maxHealth;
                healthBar.SetMaxHealth(maxHealth); 

                // setting stamina
                maxStamina = SetMaxStamina_FromStaminaLevel();
                currentStamina = maxStamina;
                staminaBar.SetMaxStamina(maxStamina); 
                

        }



        // HEALTH
        private int SetMaxHealth_FromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        // STAMINA
        private int SetMaxStamina_FromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        // TAKE DAMAGE
        public void TakeDamage(int damage)
        {
            CurrentHealth = CurrentHealth - damage;
            healthBar.SetCurrentHealth(CurrentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01" , true);

            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01" , true);
                //HANDLE PLAYER DEATH
            }

        }

        // STAMINA DAMAGE
        public void TakeStaminaDamage(float damage)
        {
            currentStamina = currentStamina - damage/2;
            staminaBar.SetCurrentStamina(currentStamina);
        }






    }


}


