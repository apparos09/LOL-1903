using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The manager for matches.
    public class MatchManager : GameplayManager
    {
        // The matchamanger instance.
        private static MatchManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        [Header("Match")]

        // The UI for the match.
        public MatchUI matchUI;

        // The current time for the match.
        public float matchTime = 0;

        // Checks if the match is paused.
        public bool matchPaused = false;

        // PLAYERS
        public PlayerMatch p1;
        public Puzzle p1Puzzle;

        public PlayerMatch p2;
        public Puzzle p2Puzzle;

        // Constructor
        private MatchManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
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

            // matchUI.timeText.text = matchTime.ToString("F2");
        }

        // Gets the instance.
        public static MatchManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<MatchManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("MatchManager (singleton)");
                        instance = go.AddComponent<MatchManager>();
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

        // Updates the player 1 display.
        public void UpdatePlayer1EquationDisplay()
        {
            matchUI.UpdatePlayer1EquationDisplay();
        }

        // Updates the player 2 display.
        public void UpdatePlayer2EquationDisplay()
        {
            matchUI.UpdatePlayer2EquationDisplay();
        }


        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}