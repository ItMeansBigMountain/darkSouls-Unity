using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


namespace AF
{



    public class HealthBar : MonoBehaviour
    {

        public Slider slider;

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value =maxHealth;
        }

        public void SetCurrentHealth(int CurrentHealth)
        {
            slider.value = CurrentHealth;
        }


    }





}

