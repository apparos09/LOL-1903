using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_CCC
{
    // The script for controlled players in matches.
    public class PlayerMatch : MonoBehaviour
    {
        // The puzzle the player is answering.
        public Puzzle puzzle;

        // The amount of points the player has.
        public float points;

        // The power the player has.
        public Power power;

        // Start is called before the first frame update
        void Start()
        {

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
        void Update()
        {

        }
    }
}