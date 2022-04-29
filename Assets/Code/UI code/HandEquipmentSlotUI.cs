using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AF
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UI_manager uI_Manager;

        public Image icon;
        WeaponItem weapon;


        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool rightHandSlot04;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;
        public bool leftHandSlot04;

        private void Awake()
        {
            uI_Manager = FindObjectOfType<UI_manager>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);

        }

        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uI_Manager.rightHandSlot01_selected = true;
            }
            else if (rightHandSlot02)
            {
                uI_Manager.rightHandSlot02_selected = true;
            }
            else if (rightHandSlot03)
            {
                uI_Manager.rightHandSlot03_selected = true;
            }
            else if (rightHandSlot04)
            {
                uI_Manager.rightHandSlot04_selected = true;
            }
            else if (leftHandSlot01)
            {
                uI_Manager.leftHandSlot01_selected = true;
            }
            else if (leftHandSlot02)
            {
                uI_Manager.leftHandSlot02_selected = true;
            }
            else if (leftHandSlot03)
            {
                uI_Manager.leftHandSlot03_selected = true;
            }
            else if (leftHandSlot04)
            {
                uI_Manager.leftHandSlot04_selected = true;
            }

        }



    }

}

