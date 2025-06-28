using ButtonMod.Tools;
using GorillaLocomotion;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ButtonMod.Behaviours
{
    public class ModInitializer : MonoBehaviour
    {
        public static bool Initialized;

        GameObject modHandler;
        GameObject lucyManagerPrefab;
        GameObject handBlockPrefab;

        Vector3 originalPos = new Vector3(-68.15f, 13.7932f, -95.9587f);
        Vector3 TeleportToPos = new Vector3(-66.1046f, 20.2213f, -81.5897f);

        Quaternion TeleportToRot = Quaternion.identity;

        private void Start() => GorillaTagger.OnPlayerSpawned(delegate
        {
            handBlockPrefab = GameObject.Find("Environment Objects/LocalObjects_Prefab/CityToBasement/DungeonEntrance/" +
                "DungeonDoor_Prefab/DungeonHandBlock_Prefab_Outside");

            modHandler = new GameObject("kinoModHandler");

            lucyManagerPrefab = GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost");

            Initialized = true;

            var hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add(Constants.NAME, Constants.VERSION);

            PhotonNetwork.LocalPlayer.CustomProperties = hash;
            PhotonNetwork.SetPlayerCustomProperties(hash);

            if (handBlockPrefab != null)
            {
                Instantiate(handBlockPrefab,
                    originalPos,
                    Quaternion.Euler(90f, 244.3172f, 0f),
                    modHandler.transform);
            }

            Instantiate(lucyManagerPrefab, modHandler.transform);

            var clonedLucyObj = GameObject.Find("kinoModHandler/Halloween Ghost(Clone)");

            if (clonedLucyObj != null)
                clonedLucyObj.SetActive(false);
        });


        private int lastTriggeredFrame = -1;

        private List<Collider> overlappingColliders = new List<Collider>(20);

        internal UnityEvent TriggeredEvent = new UnityEvent();

        private void OnTriggerEnter(Collider other)
        {
            if (overlappingColliders.Contains(other))
            {
                overlappingColliders.Add(other);
            }
            lastTriggeredFrame = Time.frameCount;
            TriggeredEvent.Invoke();
            var blocker = gameObject.AddComponent<VisionBlocker>();
            blocker.BlockVisionForTime();
        }

        private void OnTriggerExit(Collider other)
        {
            overlappingColliders.Remove(other);
        }

    }
}
