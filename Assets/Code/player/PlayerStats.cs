using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class PlayerStats : CharacterStats
    {


        // LOCK ON
        public float maximumLockOnDistance = 30f;
        public float angleThreashhold = 75f;

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

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01", true);
                //HANDLE PLAYER DEATH
            }

        }

        // STAMINA DAMAGE
        public void TakeStaminaDamage(float damage)
        {
            currentStamina = currentStamina - damage / 2;
            staminaBar.SetCurrentStamina(currentStamina);
        }






    }


}


