using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;
        

        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot backSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;


        Animator animator;

        quickSlotsUI QS_UI;
        PlayerStats playerStats;

        InputHandler inputHandler;



        private void Awake()
        {
            // Loading asset variables
            animator = GetComponent<Animator>();
            QS_UI = FindObjectOfType<quickSlotsUI>();
            inputHandler = FindObjectOfType<InputHandler>();
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            playerStats = GetComponentInParent<PlayerStats>();


            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                QS_UI.updateWeaponQuickSlots(true, weaponItem);

                #region  HANDLE LEFT WEAPON IDLE ANIMATION
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    #region  HANDLE RIGHT WEAPON IDLE ANIMATION
                    animator.CrossFade("Both Arms Empty", 0.2f);
                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                    #endregion
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                QS_UI.updateWeaponQuickSlots(false, weaponItem);

            }
        }



        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentGameModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentGameModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }


        public void CloseRightDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }


        #endregion




        #region Handle Weapon Stamina Drain

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.base_stamina * attackingWeapon.light_attack_multiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.base_stamina * attackingWeapon.heavy_attack_multiplier));
        }
        #endregion






    }
}
