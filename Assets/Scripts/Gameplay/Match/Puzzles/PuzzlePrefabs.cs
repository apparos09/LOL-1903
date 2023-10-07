using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // An instance that holds prefabs for all puzzle types.
    public class PuzzlePrefabs : MonoBehaviour
    {
        // The singleton instance.
        private static PuzzlePrefabs instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        [Header("Prefabs")]

        // The keypad mechanic.
        public KeypadMechanic keypad;

        // Constructor
        private PuzzlePrefabs()
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
        public static PuzzlePrefabs Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<PuzzlePrefabs>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Puzzle Prefabs (singleton)");
                        instance = go.AddComponent<PuzzlePrefabs>();
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