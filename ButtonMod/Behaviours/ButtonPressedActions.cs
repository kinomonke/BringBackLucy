using GorillaLocomotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ButtonMod.Behaviours
{
    public class ButtonPressedActions : MonoBehaviour
    {
        public void AllActions()
        {
            GTPlayer.Instance.disableMovement = true;
        }


    }
}
