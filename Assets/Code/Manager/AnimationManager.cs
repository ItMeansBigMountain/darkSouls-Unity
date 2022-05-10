using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AF
{
    public class AnimationManager : MonoBehaviour
    {
        public Animator anim;
        public float crossfade = 0.2f;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("IsInteracting", isInteracting);
            anim.CrossFade(targetAnim, crossfade);
        }



    }

}


