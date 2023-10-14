using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The script for controlled players in matches.
    public class PlayerMatch : MonoBehaviour
    {
        // The match manager.
        public MatchManager manager;

        // If 'true', the player uses mouse touch.
        public bool useMouseTouch = true;

        // The reticle that's used to select the next value.
        public PlayerReticle reticle;

        [Header("Match")]

        // The puzzle the player is answering.
        public Puzzle puzzle;

        // The amount of match points the player has.
        public float points;

        // The power the player has.
        public Power power;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Grab the instance.
            if (manager == null)
                manager = MatchManager.Instance;
        }

        // Called when the reticle trigger enters a collision with something.
        public virtual void OnReticleTriggerEnter(Collider other)
        {
            // ...
        }

        // Called when the reticle trigger enters a collision with something.
        public virtual void OnReticleTriggerEnter2D(Collider2D collision)
        {
            // ...
        }

        // Called when the reticle trigger enters a collision with something.
        public virtual void OnReticleTriggerStay(Collider other)
        {
            // ...
        }

        // Called when the reticle trigger enters a collision with something.
        public virtual void OnReticleTriggerStay2D(Collider2D collision)
        {
            // ...
        }

        // EQUATION //

        // POWER //
        // If the player has a power, return true.
        public bool HasPower()
        {
            // Checks if a power is set.
            bool result = power != null;

            return result;
        }

        // Sets the power to the provided power.
        public void SetPower(Power newPower)
        {
            power = newPower;
        }

        // Sets the power using the provided type.
        public void SetPower(Power.powerType type)
        {
            // Generates a power based on the type.
            switch(type)
            {
                default:
                case Power.powerType.none:
                    power = null;
                    break;

                case Power.powerType.temp:
                    // TODO: implement.
                    break;
            }
        }

        // Call to use the power.
        public void UsePower()
        {
            // Checks if the player has a power.
            if(HasPower())
            {
                // Checks if the power is active.
                if (!IsPowerActive()) // Not active.
                {
                    // If the power is usable, use it.
                    if(power.IsPowerUsable())
                    {
                        power.UsePower();
                    }

                }

            }

        }

        // Checks if the player's power is active.
        public bool IsPowerActive()
        {
            // Checks if a power is equipped.
            if(power != null)
            {
                return power.powerActive;
            }
            else // Not equipped, so always false.
            {
                return false;
            }
        }

        // TODO: should this be moved?
        // SKIP
        // Skips the equation.
        public void SkipEquation()
        {
            // TODO: implement checks to prevent the player from skipping every time.

            puzzle.SkipEquation();
        }


        // EQUATION //
        // Called when an equation is generated.
        public virtual void OnEquationGenerated()
        {
            // ...
        }

        // Called when the equation has been completed.
        public virtual void OnEquationComplete()
        {
            // ...
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            // If the mouse touch should be used, and the mouse has been clicked.
            // Maybe split this conditional into 2? 
            if(useMouseTouch)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // TODO: maybe use the callbacks?

                    // OLD
                    // This gives a delayed click.
                    // // Grabs the mouse button.
                    // util.MouseButton mb = manager.mouseTouch.MouseButton0;

                    // NEW
                    // Generates a mouse button, and sets both held and last clicked as the hovered object.
                    // This is to make sure the hit is as updated (there was a delay using last clickedb efore).

                    // The hit info for the mouse button isn't referenced.
                    util.MouseButton mb = new util.MouseButton();

                    // Held
                    mb.held = manager.mouseTouch.mouseHoveredObject;
                    mb.heldHit = manager.mouseTouch.mouseHoveredHit;
                    mb.heldHit2D = manager.mouseTouch.mouseHoveredHit2D;

                    // Last Clicked
                    mb.lastClicked = manager.mouseTouch.mouseHoveredObject;
                    mb.lastClickedHit = manager.mouseTouch.mouseHoveredHit;
                    mb.lastClickedHit2D = manager.mouseTouch.mouseHoveredHit2D;


                    // Checks last clicked.
                    if (mb.held != null)
                    {
                        // Grabs the object.
                        GameObject hit = puzzle.puzzleRender.TryHit(mb);

                        // Something was hit.
                        if (hit != null)
                        {
                            // Select the element.
                            puzzle.SelectElement(this, hit);
                        }
                    }
                }
            }
            
        }
    }
}