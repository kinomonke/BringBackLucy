﻿using BepInEx;
using BepInEx.Configuration;
using BringBackLucy.Behaviours;
using BringBackLucy.Behaviours.Audio;
using HarmonyLib;

namespace BringBackLucy
{
    [BepInPlugin(Constants.GUID, Constants.NAME, Constants.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public static ConfigEntry<bool> BringLucyBackConfig;

        private void Awake()
        {
            Instance = this;
            BringBackLucy.Tools.Logging.Logger = Logger;

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
        public const string VERSION = "1.0.1";

        public const string BUILDTIME = "10:04 | 03/07/2025";
    }
}