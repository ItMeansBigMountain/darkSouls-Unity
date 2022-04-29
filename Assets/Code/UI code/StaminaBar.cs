using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


namespace AF
{

    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxStamina(int maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        public void SetCurrentStamina(float CurrentStamina)
        {
            slider.value = CurrentStamina;
        }

    }
}