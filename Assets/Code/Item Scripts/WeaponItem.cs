using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AF
{

    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;
        public string th_idle;




        [Header("Attack Animations")]
        public string OH_light_attack_1; // one handed
        public string OH_light_attack_2;
        public string OH_heavy_attack_1;

        public string th_light_attack_01; // two handed
        public string th_light_attack_02;
        public string th_heavy_attack_01;  


        [Header("Stamina Costs")]
        public int base_stamina;
        public float light_attack_multiplier;
        public float heavy_attack_multiplier;

    }

}

