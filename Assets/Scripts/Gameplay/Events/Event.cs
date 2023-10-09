using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // An event for a scene.
    public abstract class Event : MonoBehaviour
    {
        // The name of the event.
        public string eventName = "";

        // The ID number of the event.
        public int eventId = 0;

        // A tag used to mark the event.
        public string eventTag = "";

        // Initializes the event.
        public abstract void InitalizeEvent();

        // Checks if the event has been cleared yet.
        public abstract bool CheckEvent();

        // Called when the event is completed.
        public abstract void OnEventCompleted();

    }
}