using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AF
{
    public class UI_manager : MonoBehaviour
    {

        public PlayerInventory playerInventory;
        public equipmentWindow_UI equipmentWindow_UI;

        [Header("UI Windows")]
        public GameObject equipmentWindow;
        public GameObject HUD_window;
        public GameObject weaponInventoryWindow;
        public GameObject selectWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01_selected;
        public bool rightHandSlot02_selected;
        public bool rightHandSlot03_selected;
        public bool rightHandSlot04_selected;
        public bool leftHandSlot01_selected;
        public bool leftHandSlot02_selected;
        public bool leftHandSlot03_selected;
        public bool leftHandSlot04_selected;


        [Header("Weapon Inventory")]
        public GameObject weapon_inventory_slot_prefab;
        public Transform weapon_inventory_slot_Parent;
        Weapon_inventory_Slot[] weapon_Inventory_Slots;



        private void Awake()
        {
            equipmentWindow_UI = FindObjectOfType<equipmentWindow_UI>(true);
            playerInventory = FindObjectOfType<PlayerInventory>(true);

        }


        private void Start()
        {
            weapon_Inventory_Slots = weapon_inventory_slot_Parent.GetComponentsInChildren<Weapon_inventory_Slot>(true);
            equipmentWindow_UI.LoadEquipmentOnScreen(playerInventory);
        }



        public void updateUI()
        {
            #region Weapon Inventory Slots

            for (int i = 0; i < weapon_Inventory_Slots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weapon_Inventory_Slots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weapon_inventory_slot_prefab, weapon_inventory_slot_Parent);
                        weapon_Inventory_Slots = weapon_inventory_slot_Parent.GetComponentsInChildren<Weapon_inventory_Slot>(true);
                    }
                    weapon_Inventory_Slots[i].addItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weapon_Inventory_Slots[i].ClearInventorySlot();
                }
            }



            #endregion
        }

        public void OpenSelectedWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectedWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01_selected = false;
            rightHandSlot02_selected = false;
            rightHandSlot03_selected = false;
            rightHandSlot04_selected = false;
            leftHandSlot01_selected = false;
            leftHandSlot02_selected = false;
            leftHandSlot03_selected = false;
            leftHandSlot04_selected = false;
        }

    }
}


