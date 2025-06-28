using ButtonMod.Behaviours;
using ButtonMod.Tools;
using System.Reflection;
using UnityEngine;

namespace ButtonMod.Tools
{
    public class HandButton : GTDoorTrigger
    {
        private bool isDebouncing = false;

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
                Logging.Error("kinomods: Base OnTriggerExit method not found.");
            }

            ButtonFullyPressed();
        }

        void ButtonFullyPressed()
        {
            Logging.Log("kinomods: pressed button!");

            var buttonPressedActions = gameObject.AddComponent<ButtonPressedActions>();
            buttonPressedActions.AllActions();

            StartCoroutine(DebounceCoroutine());
        }

        private System.Collections.IEnumerator DebounceCoroutine()
        {
            isDebouncing = true;
            yield return new WaitForSeconds(1f);
            isDebouncing = false;
        }
    }
}
