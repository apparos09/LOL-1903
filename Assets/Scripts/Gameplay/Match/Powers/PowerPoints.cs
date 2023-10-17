using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // Adjust the amount of points the target gets momentarily.
    public class PowerPoints : Power
    {
        [Header("Points")]

        // The target of the points power.
        public PlayerMatch target;

        // If set to 'true', the target is auto set.
        [Tooltip("Autosets the target if it's equal to null.")]
        public bool autoSetTarget = true;

        // Checks if the target is the 'user', or if it's the enemy.
        public bool targetIsUser = true;

        // the multiplier.
        public float multiplier = 1.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // If the target should be auto-set, and the target is null.
            if(autoSetTarget && target == null)
            {
                // Checks target.
                if(targetIsUser) // Target is user.
                {
                    target = (manager.p1 == playerMatch) ? manager.p1 : manager.p2;
                }
                else // Target is enemy.
                {
                    target = (manager.p2 == playerMatch) ? manager.p2 : manager.p1;
                }
            }
        }

        // Called when the power has been started.
        public override void OnPowerStarted()
        {
            base.OnPowerStarted();

            // Applies the point multipler.
            target.pointsMultiplier *= multiplier;
        }

        // Called when the power is finished.
        public override void OnPowerFinished()
        {
            base.OnPowerFinished();

            // Removes the point multipler.
            target.pointsMultiplier /= multiplier;
        }

        // Updates the power.
        public override void UpdatePower()
        {
            // Nothing
        }
    }
}