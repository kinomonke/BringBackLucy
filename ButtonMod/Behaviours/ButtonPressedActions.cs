using GorillaLocomotion;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BringBackLucy.Behaviours
{
    public class ButtonPressedActions : MonoBehaviour
    {
        Vector3 onPressedTeleportPos = new Vector3(-66.0787f, 21.8672f, -81.6381f);
        Quaternion onPressedTeleportRot = Quaternion.Euler(0f, 0f, 0f);


        public void AllActions()
        {
            StartCoroutine(DoAllActions());
        }

        private IEnumerator DoAllActions()
        {
            GTPlayer.Instance.disableMovement = true;
            GTPlayer.Instance.TeleportTo(onPressedTeleportPos, onPressedTeleportRot);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").SetActive(true);
            yield return new WaitForSeconds(20.35f);
            GTPlayer.Instance.disableMovement = false;
            gameObject.AddComponent<LucyManager>();
        }
    }
}