using UnityEngine;
using ButtonMod.Tools;
using System.Collections;


namespace ButtonMod.Behaviours
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
            if (NetworkSystem.Instance.InRoom)
            {
                yield return new WaitForSeconds(20.35f);

                GameObject.Find("Environment Objects")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost")?.SetActive(true);
                GameObject.Find("Environment Objects/05Maze_PersistentObjects/Ghosts/Halloween Ghost/FloatingChaseSkeleton")?.SetActive(true);
            }
        }
    }
}