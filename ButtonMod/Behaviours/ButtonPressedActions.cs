using GorillaLocomotion;
using System.Collections;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace ButtonMod.Behaviours
{
    public class ButtonPressedActions : MonoBehaviour
    {
        Vector3 onPressedTeleportPos = new Vector3(-66.0787f, 21.8672f, -81.6381f);
        Quaternion onPressedTeleportRot = Quaternion.Euler(0f, 0f, 0f);
        private GameObject lucyManager;

        private int Seconds;
        private float Timer;

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(delegate { AllActions(); });
        }
        public void AllActions()
        {
            StartCoroutine(DoAllActions());
        }

        private IEnumerator DoAllActions()
        {
            Wait();
            GTPlayer.Instance.disableMovement = true;
            GTPlayer.Instance.TeleportTo(onPressedTeleportPos, onPressedTeleportRot);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").SetActive(true);
            yield return new WaitForSeconds(20.35f);
            GTPlayer.Instance.disableMovement = false;
        }

        void Wait()
        {
            Timer = Time.deltaTime;
            if (Timer >= 1f)
            {
                Seconds++;
                Timer = 0f;
                if (Seconds >= 20)
                {
                    lucyManager = Object.Instantiate<GameObject>(GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost"));
                    lucyManager.gameObject.SetActive(true);
                    GameObject.Find("Halloween Ghost(Clone)");
                    Seconds = 0;
                }
            }
        }
    }
}