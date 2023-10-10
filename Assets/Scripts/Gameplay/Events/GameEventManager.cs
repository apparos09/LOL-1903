using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The event manager.
    public class GameEventManager : MonoBehaviour
    {
        // The list of events.
        public List<GameEvent> events = new List<GameEvent>();

        // Initializes the events.
        [Tooltip("Auto-calls the event initializations in the Start function.")]
        public bool callEventInits = true;

        // Calls the complete functions if events are cleared. Events are removed if they are cleared.
        [Tooltip("Calls the complete function on cleared events. Cleared events are removed from the list.")]
        public bool callEventCompletes = true;

        // Start is called before the first frame update
        void Start()
        {
            // Calls the event init functions.
            if(callEventInits)
            {
                // Calls each init function.
                foreach(GameEvent e in events)
                {
                    e.InitalizeEvent();
                }
            }
        }

        // Adds a new event. If it's already in the list, the init function won't be called either way.
        public void AddEvent(GameEvent newEvent, bool callInit)
        {
            // Checks if the event is already in the list.
            if(!events.Contains(newEvent))
            {
                // Add to the list.
                events.Add(newEvent);

                // Call the init function.
                if (callInit)
                    newEvent.InitalizeEvent();
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Cleared indexes.
            Stack<int> clearedIndexes = new Stack<int>();

            // Goes through each event.
            for(int i = 0; i < events.Count; i++)
            {
                bool cleared = false;

                // Checks the event to see if it's cleared already.
                if (events[i].cleared)
                {
                    cleared = true;
                }
                else // Not cleared, so run the update to see if it is cleared.
                {
                    cleared = events[i].UpdateEvent();
                    events[i].cleared = cleared; // Set value.
                }
                

                // If the event has been cleared, push onto the indexes.
                if(cleared)
                {
                    // Call the compelte function.
                    if (callEventCompletes)
                        events[i].OnEventComplete();

                    // Store the index.
                    clearedIndexes.Push(i);
                }
                    
            }

            // Removes cleared events.
            while (clearedIndexes.Count > 0)
            {
                int index = clearedIndexes.Pop();
                events.RemoveAt(index);
            }
        }
    }
}