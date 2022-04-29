using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    public class Interactables : MonoBehaviour
    {

        public float radius = 0.6f;
        public string interactableText ;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color =Color.blue;
            Gizmos.DrawWireSphere(transform.position , radius);
        }

        public virtual void interact(PlayerManager playerManager)
        {
            // calls when we are interacting with item
            print("you interacted with an object!");
        }




    }

}

