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
        public float points = 0;

        // A multiplier for the points the player gets.
        public float pointsMultiplier = 1.0F;

        // The power the player has.
        public Power power = null;

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
        // setPlayerAsParent - sets the player as the power's parent if it's not attached to the player directly.
        public void SetPower(Power newPower, bool setPlayerAsParent = true)
        {
            // Removes the current power.
            RemovePower();

            // Sets the power.
            power = newPower;

            // The power has been set.
            if(power != null)
            {
                // Sets the player.
                power.playerMatch = this;

                // Sets the transform parent if the component isn't attached to the player.
                if (setPlayerAsParent && power.gameObject != gameObject)
                {
                    power.transform.parent = transform;
                }
            }
            
        }

        // Sets the power using the provided type.
        // setPlayerAsParent - sets the player as the power's parent if it's not attached to the player directly.
        public void SetPower(Power.powerType type, bool setPlayerAsParent = true)
        {
            // The power prefabs.
            PowerPrefabs powerPrefabs = PowerPrefabs.Instance;

            // The new power for the player.
            Power newPower = null;

            // Checks the power type.
            switch (type)
            {
                // Generates the power for the player.
                default:
                case Power.powerType.none:
                    // TODO: set to null instead of using the nothing power.
                    // newPower = Instantiate(powerPrefabs.nothing);
                    newPower = null;
                    break;

                case Power.powerType.pointsPlus: // Points Plus
                    newPower = Instantiate(powerPrefabs.pointsPlus);
                    break;

                case Power.powerType.pointsMinus: // Points Minus
                    newPower = Instantiate(powerPrefabs.pointsMinus);
                    break;

                case Power.powerType.powerTwist: // Points Twist
                    newPower = Instantiate(powerPrefabs.renderTwist);
                    break;
            }

            // Sets the power.
            SetPower(newPower, setPlayerAsParent);
        }

        // Removes the power. If 'destroyPower' is true, the power component is destroyed.
        public void RemovePower(bool destroyPower = true)
        {
            // Power is set.
            if (power != null)
            {
                // If attached to the player, only destroy the component.
                if(power.gameObject == gameObject)
                {
                    Destroy(power);
                }
                // If not attached to the player, destroy the whole object.
                else if(power.gameObject != gameObject)
                {
                    Destroy(power.gameObject);
                }
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


                    // Checks last clicked object if it's from the user's puzzle render.
                    if (mb.held != null && mb.held == puzzle.puzzleRender.gameObject)
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