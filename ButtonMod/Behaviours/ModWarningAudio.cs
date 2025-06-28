using ButtonMod.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ButtonMod.Behaviours
{
    public class ModWarningAudio : MonoBehaviour
    {
        private AudioSource audioSource;

        void Start()
        {
            if (Plugin.BringLucyBackConfig.Value)
            {
                Logging.Log("kinomods: Warning already heard, skipping audio.");
                return;
            }

            StartCoroutine(PlayWarningMessage());
        }

        private IEnumerator PlayWarningMessage()
        {
            string url = "https://raw.githubusercontent.com/kinomonke/BringBackLucy/main/ButtonMod/AudioSources/openingGameWarning.mp3";

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

                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                audioSource.loop = false;

                audioSource.Play();
                Logging.Log("kinomods: Playing Audio.");

                yield return new WaitForSeconds(clip.length);

                Plugin.BringLucyBackConfig.Value = true; // Set config to true
                Logging.Log("kinomods: Audio finished. Config updated.");
            }
        }
    }
}
