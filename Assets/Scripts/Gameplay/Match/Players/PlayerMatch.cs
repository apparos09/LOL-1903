using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The script for controlled players in matches.
    public class PlayerMatch : MonoBehaviour
    {
        // The match manager.
        public MatchManager manager;

        // If 'true', the player uses mouse touch.
        public bool useMouseTouch = true;

        // The puzzle the player is answering.
        public Puzzle puzzle;

        // The amount of points the player has.
        public float points;

        // The power the player has.
        public Power power;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Grab the instance.
            if (manager == null)
                manager = MatchManager.Instance;
        }


        // If the player has a power, return true.
        public bool HasPower()
        {
            bool result = power != null;
            return result;
        }

        // Gets the current amount of energy built up for the power.
        public float GetPowerEnergy()
        {
            // Checks if the player has a power.
            if(HasPower())
            {
                return power.energy;
            }
            else
            {
                return -1;
            }
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            // If the mouse touch should be used, and the mouse has been clicked.
            // Maybe split this conditional into 2?
            if(useMouseTouch && Input.GetKeyDown(KeyCode.Mouse0))
            {
                // TODO: maybe use the callbacks?

                // Grabs the mouse button.
                util.MouseButton mb = manager.mouseTouch.MouseButton0;

                // Checks last clicked.
                if(mb.lastClicked != null)
                {
                    puzzle.puzzleRender.CalculateHit(mb);
                }
            }
        }
    }
}