using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // An event for a scene.
    public abstract class GameEvent : MonoBehaviour
    {
        // The name of the event.
        public string eventName = "";

        // The ID number of the event.
        public int eventId = 0;

        // A tag used to mark the event.
        public string eventTag = "";

        // Set this to 'true' to show that the event is cleared.
        public bool cleared = false;

        // Initializes the event.
        public abstract void InitalizeEvent();

        // Updates an event, checking if the event has been cleared yet.
        public abstract bool UpdateEvent();

        // Called when the event is completed.
        public abstract void OnEventComplete();

    }
}