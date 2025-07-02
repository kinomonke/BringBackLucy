using BepInEx;
using BringBackLucy.Behaviours;
using BringBackLucy.Tools;
using GorillaLocomotion;
using GorillaTag.Rendering;
using HarmonyLib;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace BringBackLucy.Tools
{
    public class HandButton : GTDoorTrigger
    {
        private bool isDebouncing = false;
        private AudioSource audioSource;
        bool hasRunOnce = false;
        public static event Action OnButtonFullyPressed;
        Vector3 onPressedTeleportPos = new Vector3(-66.0787f, 21.8672f, -81.6381f);
        Quaternion onPressedTeleportRot = Quaternion.Euler(0f, 0f, 0f);

        private void OnTriggerExit(Collider other)
        {
            if (isDebouncing)
                return;

            MethodInfo baseMethod = typeof(GTDoorTrigger).GetMethod("OnTriggerExit",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (baseMethod != null)
            {
                baseMethod.Invoke(this, new object[] { other });
            }
            else
            {
                Logging.Error("kinomods: Base trigger method not found.");
            }

            ButtonFullyPressed();
        }

        void ButtonFullyPressed()
        {
            Logging.Log("kinomods: pressed button!");
            BetterDayNightManager.instance.SetTimeOfDay(0);

            gameObject.AddComponent<LucyManager>();
            StartCoroutine(PlayHorrorAudio());
            StartCoroutine(DebounceCoroutine());
            SpawnFog();
            Pressed();
            StartCoroutine(Pressed());
            OnButtonFullyPressed?.Invoke();
        }

        public IEnumerator Pressed()
        {
            GTPlayer.Instance.disableMovement = true;
            GTPlayer.Instance.TeleportTo(onPressedTeleportPos, onPressedTeleportRot);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").SetActive(true);
            yield return new WaitForSeconds(20.35f);
            GTPlayer.Instance.disableMovement = false;
        }

        private IEnumerator DebounceCoroutine()
        {
            if (hasRunOnce) yield break;

            isDebouncing = true;
            hasRunOnce = true;

            yield return new WaitForSeconds(999999f);

            isDebouncing = false;
        }

        private IEnumerator PlayHorrorAudio()
        {
            string url = "https://raw.githubusercontent.com/kinomonke/BringBackLucy/main/ButtonMod/AudioSources/onButtonPressedAudio.mp3";

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Logging.Error($"kinomods: Failed to load audio from web: {www.error}");
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip == null)
                {
                    Logging.Error("kinomods: AudioClip is null");
                    yield break;
                }

                if (audioSource == null)
                    audioSource = gameObject.AddComponent<AudioSource>();

                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                audioSource.loop = false;

                audioSource.Play();
                Logging.Log("kinomods: Playing Audio.");

                yield return new WaitForSeconds(clip.length);
                Logging.Log("kinomods: Audio finished.");
            }
        } // done

        private void SpawnFog()
        {
            if (FogVisualizer.Instance == null)
            {
                GameObject fogObj = new GameObject("FogVisualizer");
                fogObj.AddComponent<FogVisualizer>();
                Logging.Log("kinomods: Fog spawned.");
            }
            else
            {
                Logging.Log("kinomods: FogVisualizer already exists.");
            }
        } // done
    }

    public class FogVisualizer : MonoBehaviour
    {
        public float _groundFogDepthFadeSize = 22f;
        public float _groundFogHeightFadeSize = 100f;
        public float groundFogHeight = 999999f;

        public float _lastgroundFogDepthFadeSize = 22f;
        public float _lastgroundFogHeightFadeSize = 100f;
        public float lastgroundFogHeight = 999999f;

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

        private bool hasFogged;

        void Update()
        {
            if (GorillaLocomotion.GTPlayer.Instance != null && !hasFogged)
            {
                ZoneShaderSettings Forest_ZoneShaderSettings_Prefab = GameObject.Find("Forest_ZoneShaderSettings_Prefab").GetComponent<ZoneShaderSettings>();
                Forest_ZoneShaderSettings_Prefab.isDefaultValues = true;

                Traverse ForestSettingsTraverse = Traverse.Create(Forest_ZoneShaderSettings_Prefab);
                ForestSettingsTraverse.Field("groundFogHeight").SetValue(groundFogHeight);
                ForestSettingsTraverse.Field("_groundFogDepthFadeSize").SetValue(_groundFogDepthFadeSize);
                ForestSettingsTraverse.Field("_groundFogHeightFadeSize").SetValue(_groundFogHeightFadeSize);

                typeof(ZoneShaderSettings).GetMethod("ApplyValues", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(Forest_ZoneShaderSettings_Prefab, new object[] { });

                hasFogged = true;
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
    } // done

}