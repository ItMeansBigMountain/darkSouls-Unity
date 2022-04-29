using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AF
{
    public class Weapon_inventory_Slot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        public UI_manager uI_Manager;
        WeaponSlotManager weaponSlotManager;
        WeaponItem item;
        public Image icon;



        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            uI_Manager = FindObjectOfType<UI_manager>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
        }

        public void addItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;

            icon.enabled = true;
            gameObject.SetActive(true);
        }
        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }
        public void EquipThisItem()
        {

            // remove new item from inventrory


            if (uI_Manager.rightHandSlot01_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[0]);
                playerInventory.weaponsInRightHandSlot[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uI_Manager.rightHandSlot02_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[1]);
                playerInventory.weaponsInRightHandSlot[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uI_Manager.rightHandSlot03_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[2]);
                playerInventory.weaponsInRightHandSlot[2] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uI_Manager.rightHandSlot04_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[3]);
                playerInventory.weaponsInRightHandSlot[3] = item;
                playerInventory.weaponsInventory.Remove(item);
            }



            else if (uI_Manager.leftHandSlot01_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[0]);
                playerInventory.weaponsInLeftHandSlot[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uI_Manager.leftHandSlot02_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[1]);
                playerInventory.weaponsInLeftHandSlot[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uI_Manager.leftHandSlot03_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[2]);
                playerInventory.weaponsInLeftHandSlot[2] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uI_Manager.leftHandSlot04_selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[3]);
                playerInventory.weaponsInLeftHandSlot[3] = item;
                playerInventory.weaponsInventory.Remove(item);
            }


            else return;


            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlot[playerInventory.CurrentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlot[playerInventory.CurrentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uI_Manager.equipmentWindow_UI.LoadEquipmentOnScreen(playerInventory);
            uI_Manager.ResetAllSelectedSlots();

        }


    }

}


