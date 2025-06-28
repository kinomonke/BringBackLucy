using ButtonMod.Tools;
using System.Collections;
using UnityEngine;

namespace ButtonMod.Behaviours.Visual
{
    public class VisionBlocker : MonoBehaviour
    {
        private GameObject blindfold;

        public void BlockVisionForTime()
        {
            if (!NetworkSystem.Instance.InRoom)
                return;

            GameObject mainCamera = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/Main Camera");
            if (mainCamera == null)
            {
                Logging.Error("kinomods: VisionBlocker: Main Camera not found.");
                return;
            }

            // Search for the blindfold object
            for (int i = 0; i < mainCamera.transform.childCount; i++)
            {
                GameObject child = mainCamera.transform.GetChild(i).gameObject;
                if (child.name == "PropHaunt_Blindfold_ForCameras_Prefab(Clone)")
                {
                    blindfold = child;
                    break;
                }
            }

            if (blindfold != null)
            {
                StartCoroutine(BlockRoutine());
            }
            else
            {
                Logging.Error("kinomods: Blindfold object not found.");
            }
        }

        private IEnumerator BlockRoutine()
        {
            blindfold.SetActive(true);
            Debug.Log("kinomods: Vision blocked.");

            yield return new WaitForSeconds(20.35f);

            blindfold.SetActive(false);
            Debug.Log("kinomods: Vision restored.");
        }
    }
}
