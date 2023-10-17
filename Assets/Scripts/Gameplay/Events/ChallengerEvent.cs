using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace RM_EM
{
    // An event to unlock a challenger.
    public class ChallengerEvent : GameEvent
    {
        [Header("Challenger")]

        // The challenger to be unlocked.
        [Tooltip("The challenger this event is attached to.")]
        public ChallengerWorld challenger;

        // The challengers that must be beaten for this challenger to be available.
        [Tooltip("The challengers that must be beaten for this challenger to be made active.")]
        public List<ChallengerWorld> reqChallengers;


        // Start is called before the first frame update
        void Start()
        {
            if(eventTag == "")
                eventTag = "Challenger";

            // Tries to get the challenger.
            if (challenger == null)
                challenger = GetComponent<ChallengerWorld>();
        }

        // Initializes the event.
        public override void InitalizeEvent()
        {
            // Make challenger unavailable, and turn off the challenger component.
            challenger.SetChallengerAvailable(false);
            challenger.enabled = false;
        }

        // Updates the event.
        public override bool UpdateEvent()
        {
            // Checks if all challengers have been beaten.
            bool allBeaten = true;

            // Checks if there are prior challengers that need to be beatren.
            if (reqChallengers.Count > 0)
            {
                // Goes through the prior challengers.
                for (int i = 0; i < reqChallengers.Count; i++)
                {
                    // Not all challengers have been beaten.
                    if (!reqChallengers[i].defeated)
                    {
                        allBeaten = false;
                        break;
                    }
                }
            }

            // Sets the cleared parameter (not needed).
            cleared = allBeaten;

            return allBeaten;
        }

        // Completes the event.
        public override void OnEventComplete()
        {
            // Turn on the component, and make challenger availble.
            challenger.enabled = true;
            challenger.SetChallengerAvailable(true);
        }

    }
}