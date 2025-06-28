using ButtonMod.Tools;
using ButtonMod.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ButtonMod.Behaviours.Audio
{
    public class ButtonPressedAudio : MonoBehaviour
    {
        private AudioSource audioSource;


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

                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.playOnAwake = false;
                audioSource.loop = false;

                audioSource.Play();
                Logging.Log("kinomods: Playing Audio.");

                yield return new WaitForSeconds(clip.length);
                Logging.Log("kinomods: Audio finished.");
            }
        }
    }
}
