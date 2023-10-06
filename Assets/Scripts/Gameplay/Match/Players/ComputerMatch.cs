using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The script for computer players in matches.
    public class ComputerMatch : PlayerMatch
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // The computer does not use moue touch, so don't check for that.
            useMouseTouch = false;
        }

        // Runs the computer AI.
        public void RunAI()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Run the computer's AI.
            RunAI();
        }
    }
}