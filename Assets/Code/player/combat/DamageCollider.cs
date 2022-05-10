using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class DamageCollider : MonoBehaviour
    {

        Collider damageCollider;

        public int currentWeaponDamage = 25;
        
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {

            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }




            if (collision.tag == "Enemy")
            {
                EnemyStats enemyrStats = collision.GetComponent<EnemyStats>();
                if (enemyrStats != null)
                {
                    enemyrStats.TakeDamage(currentWeaponDamage);
                }
            }





        }


    }

}
