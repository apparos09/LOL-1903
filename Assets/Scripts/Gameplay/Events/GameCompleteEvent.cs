using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The event for when the game is completed.
    public class GameCompleteEvent : GameEvent
    {
        // Manager.
        public WorldManager manager;

        // Start is called before the first frame update
        void Start()
        {
            // Not set.
            if(manager == null)
                manager = WorldManager.Instance;
        }

        // Initialize event.
        public override void InitalizeEvent()
        {
            // ...
        }

        // Update Event
        public override bool UpdateEvent()
        {
            // Checks if the event is finished.
            bool finished = true;

            // Checks the challenger count to see if it's valid.
            if(manager.challengers.Count > 0)
            {
                // Checks the final challenger if available.
                if(manager.finalChallenger != null)
                {
                    // Only checks the final boss instead of all challengers.
                    finished = manager.finalChallenger.defeated;
                }
                else // Can't find final boss, so check every challnger.
                {
                    // Goes through all challengers.
                    foreach (ChallengerWorld challenger in manager.challengers)
                    {
                        // A challenger hasn't been beaten yet, so the game can't end.
                        if (!challenger.defeated)
                        {
                            finished = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No challengers found in manager. Cannot check for game end.");
                finished = false;
            }

            return finished;
        }

        // Event complete.
        public override void OnEventComplete()
        {
            // Called to complete the game.
            manager.OnGameComplete();
        }

    }
}