using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AF
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public WeaponItem currentWeapon;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;

        public GameObject currentGameModel;



        public void UnloadWeaponAndDestroy()
        {
            if (currentGameModel != null)
            {
                Destroy(currentGameModel);
            }
        }

        public void UnloadWeapon()
        {
            if (currentGameModel != null)
            {
                currentGameModel.SetActive(false);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            
            UnloadWeaponAndDestroy();


            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {

                if(parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }


                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;

            }

            currentGameModel = model;

        }

    }

}
