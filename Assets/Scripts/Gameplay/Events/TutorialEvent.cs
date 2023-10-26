using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // TODO: maybe rename this to story event?
    // TODO: this is getting annoying. Maybe I should just hard-code the tutorial triggers where needed like before?
    // An event to trigger a tutorial.
    public class TutorialEvent : GameEvent
    {
        // The gameplay manager.
        public GameplayManager manager;

        // The tutorial object.
        public Tutorial tutorial;

        // TODO: add a callback or something for the tutorial.

        // Start is called before the first frame update
        protected override void Start()
        {
            // The manager is not set.
            if(manager == null)
            {
                // Checks if the world manager is instantiated.
                if(WorldManager.Instantiated)
                {
                    manager = WorldManager.Instance;
                }
                // Checks if the match manager is instantiated.
                else if (MatchManager.Instantiated)
                {
                    manager = MatchManager.Instance;
                }
                    
            }

            // Grabs the tutorial instance.
            if (tutorial == null)
                tutorial = Tutorial.Instance;

            base.Start();
        }

        // Initializes the event.
        public override void InitalizeEvent()
        {
            base.InitalizeEvent();
        }

        // Updates the event.
        public override void UpdateEvent()
        {
            // The match number.
            int matchNumber = 0;

            // TODO: optimize
            // Checks the manager type.
            if(manager is WorldManager) // World
            {
                matchNumber = ((WorldManager)manager).GetMatchNumber();
            }
            else if(manager is MatchManager) // Match
            {
                matchNumber = (manager as MatchManager).matchNumber;
            }


            // Checks the match number.
            switch(matchNumber)
            {
                case 1: // First Match
                    break;
                case 2: // Second Match/Post First Match
                    break;
            }
        }

        // Called when the event is complete.
        public override void OnEventComplete()
        {
            base.OnEventComplete();
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}