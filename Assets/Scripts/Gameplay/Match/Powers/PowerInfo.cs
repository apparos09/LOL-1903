using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The power prefabs.
    public class PowerInfo : MonoBehaviour
    {
        // The singleton instance.
        private static PowerInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        [Header("Prefabs")]

        // The nothing power (for testing purposes only).
        public PowerNothing nothing;

        // Increases the number of points the user gets.
        public PowerPoints pointsPlus;

        // Decreases the number of points the target gets.
        public PowerPoints pointsMinus;

        // Twists the opponent's render (no use for the player since the AI isn't effected).
        public PowerTwist renderTwist;

        [Header("Symbols")]

        // The default power symbol.
        public Sprite defaultSymbol;

        // The points plus symbol.
        public Sprite pointsPlusSymbol;

        // The points minus symbol.
        public Sprite pointsMinusSymbol;

        // The render twist symbol.
        public Sprite renderTwistSymbol;

        // Constructor
        private PowerInfo()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Gets the instance.
        public static PowerInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<PowerInfo>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Power Prefabs (singleton)");
                        instance = go.AddComponent<PowerInfo>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public bool Instantiated
        {
            get
            {
                return instanced;
            }
        }
    }
}