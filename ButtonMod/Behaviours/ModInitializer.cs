using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

namespace ButtonMod.Behaviours
{
    public class ModInitializer : MonoBehaviour
    {
        public static bool Initialized;

        GameObject modHandler;
        GameObject lucyManagerPrefab;
        GameObject handBlockPrefab;

        Vector3 originalPos = new Vector3(-68.15f, 13.7932f, -95.9587f);
        bool triggered = false;

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
                GameObject instance = Instantiate(handBlockPrefab,
                    originalPos,
                    Quaternion.Euler(90f, 244.3172f, 0f),
                    modHandler.transform);
            }

            Instantiate(lucyManagerPrefab, modHandler.transform);

            var clonedLucyObj = GameObject.Find("kinoModHandler/Halloween Ghost(Clone)");
            clonedLucyObj.SetActive(false);

        });

        void Update()
        {
            // GTPlayer.Instance.TeleportTo(TeleportToPos, TeleportToRot);

            if (!triggered && Vector3.Distance(handBlockPrefab.transform.position, originalPos) > 1f)
            {
                triggered = true;
                if (triggered == true)
                {
                    GTPlayer.Instance.disableMovement = true;
                }
            }
        }
    }
}