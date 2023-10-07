using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The script for computer players in matches.
    public class ComputerMatch : PlayerMatch
    {
        [Header("Computer")]

        // The target value to be selected.
        public PuzzleValue targetValue;

        [Header("Behavior")]
        public float moveSpeed;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // The computer does not use moue touch, so don't check for that.
            useMouseTouch = false;
        }

        // RETICLE COLLISION CALLS

        // Called when the reticle trigger continually collides with something.
        public override void OnReticleTriggerStay(Collider other)
        {
            TryGetPuzzleElement(other.gameObject);
        }

        // Called when the reticle trigger continually collides with something.
        public override void OnReticleTriggerStay2D(Collider2D collision)
        {
            TryGetPuzzleElement(collision.gameObject);
        }


        // Tries to get a puzzle element from the provided object.
        public bool TryGetPuzzleElement(GameObject hitObject)
        {
            PuzzleValue pv;

            // Attempts to select the value from the hit object and see if it's the correct one.
            // Tries to get the component.
            if(hitObject.TryGetComponent<PuzzleValue>(out pv))
            {
                // If the element's value is the same as the next missing value.
                if(pv.value == puzzle.missingValues.Peek().value)
                {
                    // Selects the value since it's the correct one, and returns true.
                    puzzle.SelectValue(this, pv);

                    targetValue = null; // No target value set now.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        // Runs the computer AI.
        public void UpdateAI()
        {
            // Tries to find the value in the puzzle space so that the reticle can move towards it.

            // Gets the value needed.
            char value = puzzle.missingValues.Peek().value;

            // Gets set to 'true' if the value should be searched for.
            bool findValue = false;

            // If the target value isn't set, always search for it.
            if(targetValue == null)
            {
                findValue = true;
            }
            else
            {
                // Checks if the target value matches the desired value. If it doesn't, search for it.
                if (targetValue.value != value)
                    findValue = true;
            }

            // If the value should be searched for.
            if(findValue)
            {
                // Set to null to start off.
                targetValue = null;

                // Checks the value list.
                for (int i = 0; i < puzzle.puzzleMechanic.puzzleValues.Count; i++)
                {
                    // The value is set.
                    if (puzzle.puzzleMechanic.puzzleValues[i] != null)
                    {
                        // If this has the correct value.
                        if (puzzle.puzzleMechanic.puzzleValues[i].value == value)
                        {
                            // Sets the target value.
                            targetValue = puzzle.puzzleMechanic.puzzleValues[i];
                            break;
                        }
                    }
                }
            }
            

            // If the target value is still not set, do nothing.
            if (targetValue == null)
                return;

            // NOTE: this moves to where the value physically is, which is outside of the game view.
            // The value is projected through a quad, which means it visually won't look right.
            // I won't fix this unless I plan to actually show reticles.

            Vector3 newPos = Vector3.MoveTowards(reticle.transform.position, targetValue.transform.position, 1);
            reticle.transform.position = newPos;

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Run the computer's AI.
            UpdateAI();
        }
    }
}