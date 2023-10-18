using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The script for computer players in matches.
    public class ComputerMatch : PlayerMatch
    {
        [Header("Computer")]

        // The difficulty of the computer.
        public int difficulty = 0;

        // The target value to be selected.
        public PuzzleValue targetValue;

        [Header("AI")]

        // The wait timer between movements.
        public float waitTimer = 0.0F;

        // The max wait time.
        public float waitTimeMax = 2.5F;

        // The move speed.
        public float moveSpeed = 5.0F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // The computer does not use moue touch, so don't check for that.
            useMouseTouch = false;
        }

        // Sets the difficulty of the computer.
        public void SetDifficulty(int difficulty)
        {
            this.difficulty = difficulty;

            // TODO: implement difficulty changes.
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

        // Called when an equation is generated.
        public override void OnEquationGenerated()
        {
            base.OnEquationGenerated();
            waitTimer = waitTimeMax;
        }

        // Called when the equation has been completed.
        public override void OnEquationComplete()
        {
            base.OnEquationComplete();
            waitTimer = waitTimeMax;
        }


        // Runs the computer AI.
        public void UpdateAI()
        {
            // Checks if the wait timer is going.
            if(waitTimer > 0.0F)
            {
                // Reduce timer.
                waitTimer -= Time.deltaTime;

                // The wait timer is negative, so set it to 0.
                if (waitTimer < 0.0F)
                {
                    waitTimer = 0.0F;
                }

                // Still waiting, so return.
                return;
                    
            }

            // TODO: prioritize closest value.
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

                // TODO: have the computer sometimes get questions wrong?

                // The value options.
                List<PuzzleValue> valueOptions = new List<PuzzleValue>();

                // Checks the value list.
                for (int i = 0; i < puzzle.puzzleMechanic.puzzleValues.Count; i++)
                {
                    // The value is set.
                    if (puzzle.puzzleMechanic.puzzleValues[i] != null)
                    {
                        // If this has the correct value.
                        if (puzzle.puzzleMechanic.puzzleValues[i].value == value)
                        {
                            // Checks if the loop should be broken early.
                            bool breakEarly = false;

                            // Adds to the list of valeu options.
                            valueOptions.Add(puzzle.puzzleMechanic.puzzleValues[i]);

                            // Checks the puzzle type.
                            switch (puzzle.puzzle)
                            {
                                // If it's a keypad, there's one of every value, so break early.
                                case Puzzle.puzzleType.keypad:
                                    breakEarly = true;
                                    break;

                                default:
                                    breakEarly = false;
                                    break;
                            }

                            // If the loop should be broken early.
                            if(breakEarly)
                                break;
                        }
                    }
                }


                // Original
                // targetValue = puzzle.puzzleMechanic.puzzleValues[i];

                // Checks the number of value options. If tehre's only one, select that value.
                if (valueOptions.Count == 1)
                {
                    // Sets the target value.
                    targetValue = valueOptions[0];
                }
                else if (valueOptions.Count > 1) // There's multiple values, so check the closet one.
                {
                    targetValue = valueOptions[0];
                    
                    // Goes through all values.
                    foreach(PuzzleValue pv in valueOptions)
                    {
                        // Gets the current distance and the comparable distance.
                        float currDist = Vector3.Distance(reticle.transform.position, targetValue.transform.position);
                        float compDist = Vector3.Distance(reticle.transform.position, pv.transform.position);

                        // If the compared distance is smaller than the current distance, use the compared distance.
                        if(compDist < currDist)
                        {
                            // Set to the target value.
                            targetValue = pv;
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

            Vector3 newPos = Vector3.MoveTowards(reticle.transform.position, targetValue.transform.position, moveSpeed * Time.deltaTime);
            reticle.transform.position = newPos;

            // TODO: set something to use powers and skips.
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