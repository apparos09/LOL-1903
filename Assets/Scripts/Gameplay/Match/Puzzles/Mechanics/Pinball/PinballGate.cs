using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The gate for the pinball game.
    public class PinballGate : MonoBehaviour
    {
        // The pinball mechanic.
        public PinballMechanic mechanic;

        // The left door and right door.
        public GameObject leftDoor;
        public GameObject rightDoor;

        // Gets set to 'true' if the gate is open.
        public bool openGate = false;

        // The max weight that can be handled before the gate opens.
        public float maxWeight = 100.0F;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Opens the gate.
        public void OpenGate()
        {
            openGate = true;
        }

        // Closes the gate.
        public void CloseGate()
        {
            openGate = false;
        }

        // Calculates the weight being applied on the gate.
        public float CalculateAppliedWeight()
        {
            return 0;
        }

        // Update is called once per frame
        void Update()
        {
            // Checks that the gate is closed.
            if(!openGate)
            {
                // Checks if the weight is too much, causing the gate to open.
                if (CalculateAppliedWeight() > maxWeight)
                {
                    OpenGate();
                }
            }
            
        }
    }
}