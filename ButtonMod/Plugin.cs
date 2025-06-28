using BepInEx;
using BepInEx.Configuration;
using ButtonMod.Behaviours;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ButtonMod
{
    [BepInPlugin(Constants.GUID, Constants.NAME, Constants.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;

        private Plugin()
        {
            Instance = this;
            Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, Constants.GUID);
            gameObject.AddComponent<ModInitializer>();
            gameObject.AddComponent<FogVisualizer>();
        }
    }
    public class Constants
    {
        public const string GUID = "kino.bringbacklucy";
        public const string NAME = "BringBackLucy";
        public const string VERSION = "1.0.0";
    }
}