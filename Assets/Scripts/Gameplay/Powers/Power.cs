using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // A power to be used by a player.
    public abstract class Power : MonoBehaviour
    {
        // The power type.
        public enum powerType { none, pointsPlus, pointsMinus, powerTwist }

        // The total number of powers (including the 'none' type).
        public const int POWER_TYPE_COUNT = 4;

        // The match manager.
        public MatchManager manager;

        // The player the power is for.
        public PlayerMatch playerMatch;

        [Header("Power")]

        // The power type.
        public powerType power = powerType.none;

        // The name of the power.
        public string powerName = string.Empty;

        // The power description.
        public string powerDesc = string.Empty;

        // The energy for the power (0-1.0).
        public float energy = 0;

        // The maximum amount of energy for the power.
        public float energyMax = 100.0F;

        // Gets set to 'true' when the power is active.
        public bool powerActive = false;

        [Header("Power/Speed")]
        // The increment for power filling.
        [Tooltip("The increment for power filling.")]
        public float powerFillInc = 10.0F;

        // The rate that the power fills that.
        [Tooltip("The rate for power filling.")]
        public float powerFillRate = 1.0F;

        // The decrement at which the power decays when used.
        [Tooltip("The decrement for power depletion.")]
        public float depletionDec = 1.0F;

        // The rate at which the power decays when used.
        [Tooltip("The rate for power depletion.")]
        public float depletionRate = 1.0F;
      

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // The match manager.
            if (manager == null)
                manager = MatchManager.Instance;
        }

        // INFO
        // Gets the power name.
        public static string GetPowerTypeName(powerType power, bool shortHand)
        {
            return PowerInfo.GetPowerTypeName(power, shortHand);
        }

        // Gets the power description.
        public static string GetPowerTypeDescription(powerType power)
        {
            return PowerInfo.GetPowerTypeDescription(power);
        }


        // FUNCTIONALITY
        // Checks if the power is usable.
        public bool IsPowerUsable()
        {
            bool result = energy >= energyMax;
            return result;
        }

        // Uses the power.
        public void UsePower()
        {
            powerActive = true;
            OnPowerStarted();
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
            energy += powerFillInc * powerFillRate;
            energy = Mathf.Clamp(energy, 0, energyMax);
        }

        // Called when the power is started.
        public virtual void OnPowerStarted()
        {
            manager.OnPowerStarted(this);
        }

        // Called when the power is over.
        public virtual void OnPowerFinished()
        {
            manager.OnPowerFinished(this);
        }

        // Called to update the power when it's active. 
        public abstract void UpdatePower();

        // Update is called once per frame
        protected virtual void Update()
        {
            // If the match isn't paused.
            if(!manager.MatchPaused)
            {
                // If the power is active.
                if (powerActive)
                {
                    // TODO: should I change the order here?
                    // Update the power.
                    UpdatePower();

                    // Called while the power is active.
                    manager.OnPowerActive(this);

                    // Reduce the energy.
                    energy -= depletionDec * depletionRate * Time.deltaTime;

                    // If the energy is less than or equal to 0, stop the power.
                    if (energy <= 0)
                    {
                        energy = 0;
                        powerActive = false;
                        OnPowerFinished();
                    }
                }
            }
            
        }
    }
}