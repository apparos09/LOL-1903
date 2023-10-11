using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The match tutorials.
    public class MatchTutorial : Tutorial
    {
        // The singleton instance.
        private static MatchTutorial instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        // THe match manager.
        public MatchManager manager;

        // Constructor
        private MatchTutorial()
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
        protected override void Start()
        {
            base.Start();

            // Gets the match manager.
            if (manager == null)
                manager = MatchManager.Instance;
        }

        // Gets the instance.
        public static MatchTutorial Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<MatchTutorial>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Match Tutorial (singleton)");
                        instance = go.AddComponent<MatchTutorial>();
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

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}