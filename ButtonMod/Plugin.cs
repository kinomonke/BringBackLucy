using BepInEx;
using BepInEx.Configuration;
using ButtonMod.Behaviours;
using ButtonMod.Behaviours.Audio;
using HarmonyLib;

namespace ButtonMod
{
    [BepInPlugin(Constants.GUID, Constants.NAME, Constants.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public static ConfigEntry<bool> BringLucyBackConfig;

        private void Awake()
        {
            Instance = this;
            ButtonMod.Tools.Logging.Logger = Logger;

            BringLucyBackConfig = Config.Bind(
                "General",
                "HasHeardWarning", false,
                "True if the player has already heard the warning audio.");

            Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, Constants.GUID);

            gameObject.AddComponent<ModInitializer>();
            gameObject.AddComponent<ModWarningAudio>();
        }
    }

    public static class Constants
    {
        public const string GUID = "kino.bringbacklucy";
        public const string NAME = "BringBackLucy";
        public const string VERSION = "1.0.0";
    }
}