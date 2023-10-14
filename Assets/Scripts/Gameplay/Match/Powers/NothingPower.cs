using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // A power that does nothing. This is only for testing purposes.
    public class PowerNothing : Power
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
            powerName = "Power Nothing";
            power = powerType.none;
        }

        // Called to update the power.
        public override void UpdatePower()
        {
            // Does nothing.
        }
    }
}