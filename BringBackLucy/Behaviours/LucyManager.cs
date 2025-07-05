using UnityEngine;
using BringBackLucy.Tools;
using System.Collections;

namespace BringBackLucy.Behaviours
{
    public class LucyManager : MonoBehaviour
    {
        void OnEnable()
        {
            HandButton.OnButtonFullyPressed += OnButtonPressed;
        }

        void OnDisable()
        {
            HandButton.OnButtonFullyPressed -= OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            StartCoroutine(TryEnableGhosts());
        }

        IEnumerator TryEnableGhosts()
        {
            if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.GameModeString.Contains("MODDED"))
            {
                yield return new WaitForSeconds(20.15f);

                GameObject.Find("Environment Objects")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost/FloatingChaseSkeleton")?.SetActive(true);

                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost/FloatingChaseSkeleton - BAYOU only")?.SetActive(false);
            }
            else
            {
                GameObject.Find("Environment Objects")?.SetActive(false);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects")?.SetActive(false);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts")?.SetActive(false);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost")?.SetActive(false);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost/FloatingChaseSkeleton")?.SetActive(false);
            }
        }
    }
}