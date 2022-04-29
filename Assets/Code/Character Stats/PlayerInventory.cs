using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{


    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;


        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        public WeaponItem unarmedWeapon;

        public static int amount = 4;
        public WeaponItem[] weaponsInRightHandSlot = new WeaponItem[amount];
        public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[amount];


        public int CurrentRightWeaponIndex = -1;
        public int CurrentLeftWeaponIndex = -1;


        public List<WeaponItem> weaponsInventory;






        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

        }

        private void Start()
        {
            rightWeapon = weaponsInRightHandSlot[0];
            leftWeapon = weaponsInLeftHandSlot[0];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }




        // BUG SKIPPING INDEX 2


        public void changeRightWeapon()
        {
            CurrentRightWeaponIndex = CurrentRightWeaponIndex + 1;
            if (CurrentRightWeaponIndex == 0 && weaponsInRightHandSlot[0] != null)
            {
                rightWeapon = weaponsInRightHandSlot[CurrentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[CurrentRightWeaponIndex], false);
            }
            else if (CurrentRightWeaponIndex == 0 && weaponsInRightHandSlot[0] == null)
            {
                CurrentRightWeaponIndex = CurrentRightWeaponIndex + 1;
            }
            else if (CurrentRightWeaponIndex == 1 && weaponsInRightHandSlot[1] != null)
            {
                rightWeapon = weaponsInRightHandSlot[CurrentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[CurrentRightWeaponIndex], false);
            }
            else
            {
                CurrentRightWeaponIndex = CurrentRightWeaponIndex + 1;
            }
            if (CurrentRightWeaponIndex > weaponsInRightHandSlot.Length - 1)
            {
                CurrentRightWeaponIndex = -1;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            }

        }


        public void changeLeftWeapon()
        {
            CurrentLeftWeaponIndex = CurrentLeftWeaponIndex + 1;
            if (CurrentLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[CurrentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[CurrentLeftWeaponIndex], true);
            }
            else if (CurrentLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] == null)
            {
                CurrentLeftWeaponIndex = CurrentLeftWeaponIndex + 1;
            }
            else if (CurrentLeftWeaponIndex == 1 && weaponsInLeftHandSlot[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[CurrentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[CurrentLeftWeaponIndex], true);
            }
            else
            {
                CurrentLeftWeaponIndex = CurrentLeftWeaponIndex + 1;
            }


            if (CurrentLeftWeaponIndex > weaponsInLeftHandSlot.Length - 1)
            {
                CurrentLeftWeaponIndex = -1;
                leftWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            }

        }


    }




}


