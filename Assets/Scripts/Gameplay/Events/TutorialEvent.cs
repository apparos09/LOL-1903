using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // TODO: implement.
    // An event to trigger a tutorial.
    public class TutorialEvent : GameEvent
    {
        // The tutorial object.
        public Tutorial tutorial;

        // TODO: add a callback or something for the tutorial.

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the tutorial instance.
            if (tutorial == null)
                tutorial = Tutorial.Instance;
        }

        // Initializes the event.
        public override void InitalizeEvent()
        {
            throw new System.NotImplementedException();
        }

        // Updates the event.
        public override bool UpdateEvent()
        {
            throw new System.NotImplementedException();
        }

        // Called when the event is complete.
        public override void OnEventComplete()
        {
            throw new System.NotImplementedException();
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}