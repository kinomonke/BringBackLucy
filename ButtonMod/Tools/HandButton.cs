using ButtonMod.Behaviours;
using System.Reflection;
using UnityEngine;
// Thank you to TheGraze for help with the Button!
namespace ButtonMod.Tools
{
    public class HandButton : GTDoorTrigger
    {
        private void OnTriggerExit(Collider other)
        {
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
        }
    }
}