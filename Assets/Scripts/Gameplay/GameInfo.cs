using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // This script is used for game info that's shared between all scenes.
    public class GameInfo : MonoBehaviour
    {
        // The singleton instance.
        private static GameInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Shared Game Info")]
        // The game time.
        public float gameTime = 0.0F;

        // Constructor
        private GameInfo()
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

        // Start is called before the first frame update
        private void Start()
        {
            // Don't destroy this game object on load.
            DontDestroyOnLoad(gameObject);
        }

        // Gets the instance.
        public static GameInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<GameInfo>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Game Info (singleton)");
                        instance = go.AddComponent<GameInfo>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}