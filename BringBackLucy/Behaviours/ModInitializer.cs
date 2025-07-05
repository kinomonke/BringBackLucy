using UnityEngine;
using UnityEngine.Events;
using BringBackLucy.Tools;

namespace BringBackLucy.Behaviours
{
    public class ModInitializer : MonoBehaviour
    {
        public static bool Initialized;

        GameObject modHandler;
        GameObject handBlockPrefab;

        Vector3 originalPos = new Vector3(-68.15f, 13.7932f, -95.9587f);

        private void Start() => GorillaTagger.OnPlayerSpawned(delegate
        {
            handBlockPrefab = GameObject.Find("Environment Objects/LocalObjects_Prefab/CityToBasement/DungeonEntrance/" +
                "DungeonDoor_Prefab/DungeonHandBlock_Prefab_Outside");

            modHandler = new GameObject("kinoModHandler");

            if (handBlockPrefab != null)
            {
                var instantiatedHandBlock = Instantiate(
                    handBlockPrefab,
                    originalPos,
                    Quaternion.Euler(90f, 244.3172f, 0f),
                    modHandler.transform);

                var doorTrigger = instantiatedHandBlock.GetComponent<GTDoorTrigger>();
                if (doorTrigger != null)
                {
                    Destroy(doorTrigger);
                    instantiatedHandBlock.AddComponent<HandButton>();
                }
            }

            Initialized = true;
            Logging.Log("kinomods: Sucessfully Initialized!");
        });
    }
}