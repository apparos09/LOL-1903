using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // TODO: maybe rename this to story event?
    // An event to trigger a tutorial.
    public class TutorialEvent : GameEvent
    {
        // The tutorial object.
        public Tutorial tutorial;

        // TODO: add a callback or something for the tutorial.

        // Start is called before the first frame update
        protected override void Start()
        {
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
            // ...
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