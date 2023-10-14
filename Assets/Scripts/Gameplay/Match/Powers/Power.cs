using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // A power to be used by a player.
    public class Power : MonoBehaviour
    {
        // The power type.
        public enum powerType { none, temp }

        // The power type.
        public powerType power  = Power.powerType.none;

        // The energy for the power (0-1.0).
        public float energy = 0;

        // The maximum amount of energy for the power.
        public float energyMax = 100.0F;

        // The rate at which the power decays when used.
        public float depletionRate = 1.0F;

        // Gets set to 'true' when the power is active.
        public bool powerActive = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Gets the power fill percentage.
        public float GetPowerFillPercentage()
        {
            // The resulting value.
            float result = 0.0F;

            // Checks if energyMax is valid.
            if(energyMax <= 0)
            {
                result = 1.0F;
            }
            else
            {
                result = energy / energyMax;
            }

            return result;
        }

        // Increases the power energy by a set amount.
        public void IncreasePowerEnergy()
        {
            energy += 10.0F;
            energy = Mathf.Clamp(energy, 0, energyMax);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}