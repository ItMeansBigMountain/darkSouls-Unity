using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AF
{
    public class equipmentWindow_UI : MonoBehaviour
    {
        public bool rightHandSlot_01_Selected;
        public bool rightHandSlot_02_Selected;
        public bool rightHandSlot_03_Selected;
        public bool rightHandSlot_04_Selected;
        public bool leftHandSlot_01_Selected;
        public bool leftHandSlot_02_Selected;
        public bool leftHandSlot_03_Selected;
        public bool leftHandSlot_04_Selected;


        public HandEquipmentSlotUI[] handEquipmentSlotUI;


        private void Awake()
        {
            handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>(true);
        }



        public void LoadEquipmentOnScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                // RIGHT HAND
                if (handEquipmentSlotUI[i].rightHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[0]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[1]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot03)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[2]);
                }
                else if (handEquipmentSlotUI[i].rightHandSlot04)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlot[3]);
                }


                // LEFT HAND
                else if (handEquipmentSlotUI[i].leftHandSlot01)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[0]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot02)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[1]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot03)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[2]);
                }
                else if (handEquipmentSlotUI[i].leftHandSlot04)
                {
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[3]);
                }
            }
        }



        public void Select_rightHandSlot_01()
        {
            rightHandSlot_01_Selected = true;
        }
        public void Select_rightHandSlot_02()
        {
            rightHandSlot_02_Selected = true;
        }
        public void Select_rightHandSlot_03()
        {
            rightHandSlot_03_Selected = true;
        }
        public void Select_rightHandSlot_04()
        {
            rightHandSlot_04_Selected = true;
        }

        public void Select_leftHandSlot_01()
        {
            leftHandSlot_01_Selected = true;
        }
        public void Select_leftHandSlot_02()
        {
            leftHandSlot_02_Selected = true;
        }
        public void Select_leftHandSlot_03()
        {
            leftHandSlot_03_Selected = true;
        }
        public void Select_leftHandSlot_04()
        {
            leftHandSlot_04_Selected = true;
        }



    }

}


