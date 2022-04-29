using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{

    public class WeaponPickup : Interactables
    {

        public WeaponItem weapon;


        public override void interact(PlayerManager playerManager)
        {
            base.interact(playerManager);
            pickupItem(playerManager);

        }

        private void pickupItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimateHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimateHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("pickupItem", true);
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractable_GameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemInteractable_GameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractable_GameObject.SetActive(true);
            Destroy(gameObject);
        }


    }

}

