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
            StartCoroutine(PlayAudioFromWeb());
        }

        private IEnumerator PlayAudioFromWeb()
        {
            string url = "https://raw.githubusercontent.com/kinomonke/BringBackLucy/main/ButtonMod/AudioSources/openingGameWarningMessage.mp3";

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[BringBackLucy] Failed to load audio from web: {www.error}");
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip == null)
                {
                    Debug.LogError("[BringBackLucy] AudioClip is null");
                    yield break;
                }

                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                audioSource.loop = false;

                audioSource.Play();
                Debug.Log("[BringBackLucy] Playing web-loaded warning audio.");
            }
        }
    }
}
