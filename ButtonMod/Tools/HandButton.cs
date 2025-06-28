using ButtonMod.Behaviours;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace ButtonMod.Tools
{
    public class HandButton : GTDoorTrigger
    {
        private void OnTriggerExit(Collider other)
        {
            // Call the base method using reflection
            //this makes sure the model still moves
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
            Logging.Log("kinomods: HandButton OnTriggerExit");
            ButtonFullyPressed();
        }

        void ButtonFullyPressed()
        {
            Logging.Log("kinomods: pressed button!");
            gameObject.AddComponent<FogVisualizer>();
        }
    }
}