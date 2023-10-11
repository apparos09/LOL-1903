using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // An area switch event.
    public class AreaSwitchEvent : GameEvent
    {
        [Header("Area Switch")]

        // Checks for the manager.
        public WorldManager manager;

        // The area this event belongs to.
        public Area area;

        // Start is called before the first frame update
        void Start()
        {
            eventTag = "Area Switch";

            // Manager.
            if (manager == null)
                manager = WorldManager.Instance;

            // Tries to get the area if it's not set.
            if (area == null)
                area = GetComponent<Area>();
        }

        // Initializes the event.
        public override void InitalizeEvent()
        {
            // Turn off the next button.
            manager.worldUI.nextAreaButton.interactable = false;
        }

        // Updates the event.
        public override bool UpdateEvent()
        {
            // Checks if the area is finished.
            bool finished = true;

            // Checks if there are challengers saved to this area.
            if(area.challengers.Count > 0)
            {
                // Goes through each challenger.
                for(int i = 0; i < area.challengers.Count; i++)
                {
                    // A challenger hasn't be beaten yet, so the area is not finished.
                    if (!area.challengers[i].defeated)
                    {
                        finished = false;
                        break;
                    }
                }
            }
            else
            {
                finished = true;
            }

            // Not necessary.
            cleared = finished;

            return finished;
        }

        // Called when the event is complete.
        public override void OnEventComplete()
        {
            // Checks if the manager contains the area.
            if(manager.areas.Contains(area))
            {
                // Turn on the next button if this isn't the last area.
                if(manager.areas.IndexOf(area) != manager.areas.Count - 1)
                    manager.worldUI.nextAreaButton.interactable = true;
            }

        }
    }
}