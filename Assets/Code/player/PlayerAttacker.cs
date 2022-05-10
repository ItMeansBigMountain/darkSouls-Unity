using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public class PlayerAttacker : MonoBehaviour
    {

        AnimateHandler animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimateHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();

        }


        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_light_attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_light_attack_2, true);
                }
                else if (lastAttack == weapon.th_light_attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.th_light_attack_02, true);
                }
            }
        }

        public void handleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.th_light_attack_01, true);
                lastAttack = weapon.th_light_attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_light_attack_1, true);
                lastAttack = weapon.OH_light_attack_1;

            }
        }

        public void handleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.th_heavy_attack_01, true);
                lastAttack = weapon.th_heavy_attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_heavy_attack_1, true);
                lastAttack = weapon.OH_heavy_attack_1;
            }


        }

    }

}
