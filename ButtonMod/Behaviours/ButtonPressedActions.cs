using ButtonMod.Behaviours.Audio;
using ButtonMod.Behaviours.Visual;
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
        Vector3 onPressedTeleportPos = new Vector3(-66.0787f, 21.8672f, -81.6381f);
        Quaternion onPressedTeleportRot = Quaternion.Euler(0f, 0f, 0f);

        public void AllActions()
        {
            GTPlayer.Instance.TeleportTo(onPressedTeleportPos, onPressedTeleportRot);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").SetActive(true);
        }
    }
}