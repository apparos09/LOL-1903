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

            useMouseTouch = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}