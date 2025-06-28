using BepInEx;
using GorillaTag.Rendering;
using HarmonyLib;
using System;
using UnityEngine;
// Thank you to iiDk for the Fog code! ALL CREDIT GOES TO HIM FOR THIS!
namespace ButtonMod.Behaviours.Visual
{
    public class FogVisualizer : MonoBehaviour
    {
        public float _groundFogDepthFadeSize = 22f;
        public float _groundFogHeightFadeSize = 100f;
        public float groundFogHeight = 450f;

        public float _lastgroundFogDepthFadeSize = 22f;
        public float _lastgroundFogHeightFadeSize = 100f;
        public float lastgroundFogHeight = 450f;

        public bool isVisible = false;
        public float visibleDelay = 0f;
        public ZoneShaderSettings currentZoneSettings;

        public static FogVisualizer Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            // Try to find ZoneShaderSettings if we don't have one
            if (currentZoneSettings == null)
            {
                var zoneObj = GameObject.Find("Forest_ZoneShaderSettings_Prefab");
                if (zoneObj != null)
                {
                    currentZoneSettings = zoneObj.GetComponent<ZoneShaderSettings>();
                }
            }

            // Apply fog settings continuously
            if (currentZoneSettings != null)
            {
                currentZoneSettings.isDefaultValues = true;

                Traverse zoneTraverse = Traverse.Create(currentZoneSettings);
                zoneTraverse.Field("groundFogHeight").SetValue(groundFogHeight);
                zoneTraverse.Field("_groundFogDepthFadeSize").SetValue(_groundFogDepthFadeSize);
                zoneTraverse.Field("_groundFogHeightFadeSize").SetValue(_groundFogHeightFadeSize);

                typeof(ZoneShaderSettings).GetMethod("ApplyValues",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic)
                    .Invoke(currentZoneSettings, null);
            }
        }

        void OnGUI()
        {
            if (UnityInput.Current.GetKey(KeyCode.I) && Time.time > visibleDelay)
            {
                isVisible = !isVisible;
                visibleDelay = Time.time + 0.2f;
            }

            if (!isVisible)
                return;

            _groundFogDepthFadeSize = GUI.HorizontalSlider(new Rect(10f, 10f, 200f, 15f), _groundFogDepthFadeSize, 0f, 150f);
            GUI.Label(new Rect(220f, 10f, 800f, 25f), "_groundFogDepthFadeSize: " + _groundFogDepthFadeSize.ToString("F0"));

            _groundFogHeightFadeSize = GUI.HorizontalSlider(new Rect(10f, 25f, 200f, 15f), _groundFogHeightFadeSize, 0f, 150f);
            GUI.Label(new Rect(220f, 25f, 800f, 25f), "_groundFogHeightFadeSize: " + _groundFogHeightFadeSize.ToString("F0"));

            groundFogHeight = GUI.HorizontalSlider(new Rect(10f, 40f, 200f, 15f), groundFogHeight, 0f, 1000f);
            GUI.Label(new Rect(220f, 40f, 800f, 25f), "groundFogHeight: " + groundFogHeight.ToString("F0"));

            // Check if values changed to force reapply
            if (_lastgroundFogDepthFadeSize != _groundFogDepthFadeSize ||
                _lastgroundFogHeightFadeSize != _groundFogHeightFadeSize ||
                lastgroundFogHeight != groundFogHeight)
            {
                currentZoneSettings = null; // Force refind settings on next update
            }

            _lastgroundFogDepthFadeSize = _groundFogDepthFadeSize;
            _lastgroundFogHeightFadeSize = _groundFogHeightFadeSize;
            lastgroundFogHeight = groundFogHeight;
        }
    }
}