using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace AF
{
    public class quickSlotsUI : MonoBehaviour
    {

        public Image rightWeaponIcon; 
        public Image leftWeaponIcon; 


        public void updateWeaponQuickSlots( bool isLeft , WeaponItem weapon)
        {
            // right handed weapon
            if (isLeft == false)
            {
                if (weapon.itemIcon != null) //null check
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }

            }
            else // left handed weapon
            {
                if (weapon.itemIcon != null) //null check
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }

            }


        }



    }

}