using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_EM
{
    // The manager for the world.
    public class WorldManager : GameplayManager
    {
        // The worldmanager singleton instance.
        private static WorldManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        [Header("World")]
        // The world UI.
        public WorldUI worldUI;

        // Used to pause the world.
        public bool worldPaused = false;

        // Constructor
        private WorldManager()
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

            // Hide the challenger UI.
            worldUI.HideChallengeUI();

            // Searches for the world info.
            WorldInfo info = FindObjectOfType<WorldInfo>();

            // If the info object was found.
            if(info != null)
            {
                gameTime = info.gameTime;

                // TODO: do more.

                // Destroy the game object.
                Destroy(info.gameObject);
            }
        }

        // Gets the instance.
        public static WorldManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<WorldManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("WorldManager (singleton)");
                        instance = go.AddComponent<WorldManager>();
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

        // SETTINGS //

        // PAUSING
        // Pauses the game, and the match overall.
        public override void SetPausedGame(bool paused)
        {
            base.SetPausedGame(paused);
            SetPausedWorld(paused);
        }

        // Pauses the match, and only the match.
        public void SetPausedWorld(bool paused)
        {
            // Set paused.
            worldPaused = paused;

            // Checks if paused or not.
            if (paused)
            {
                // ...
            }
            else
            {
                // ...
            }


        }

        // Pauses the world.
        public void PauseWorld()
        {
            SetPausedWorld(true);
        }

        // Unpauses the world.
        public void UnpauseWorld()
        {
            SetPausedWorld(false);
        }

        // Toggles the world being paused.
        public void TogglePausedWorld()
        {
            SetPausedWorld(!worldPaused);
        }


        // CHALLENGE //
        // Accept the challenge.
        public void AcceptChallenge(ChallengerWorld challenger)
        {
            // Sets the provided challenger.
            worldUI.challengeUI.challenger = challenger;

            // Goes to the match scene.
            ToMatchScene();
        }

        // Accept the challenge.
        public void AcceptChallenge()
        {
            AcceptChallenge(worldUI.challengeUI.challenger);
        }

        // Decline the challenge.
        public void DeclineChallenge()
        {
            worldUI.HideChallengeUI();
        }

        // SCENE TRANSITIONS
        // Goes to the match scene. Call AcceptChallenge() if a match info object should be created.
        public void ToMatchScene()
        {
            // Creates an object and provides the match info.
            GameObject newObject = new GameObject("Match Info");
            MatchInfo info = newObject.AddComponent<MatchInfo>();

            // Don't destroy the object on load.
            DontDestroyOnLoad(newObject);

            // TODO: add content.
            // Grabs the chalelnger.
            ChallengerWorld challenger = worldUI.challengeUI.challenger;

            info.gameTime = gameTime;

            // PUZZLE/CHALLENGE INFO
            // Sets the puzzle type.
            info.puzzleType = challenger.puzzleType;

            // Exponents
            // Base, Mult Same, Expo By Expo
            info.baseExpoRate = challenger.baseExpoRate;
            info.multSameRate = challenger.multSameRate; 
            info.expoByExpoRate = challenger.expoByExpoRate;

            // Mult Diff, Zero, Negative
            info.multDiffRate = challenger.multDiffRate;
            info.zeroRate = challenger.zeroRate;
            info.negativeRate = challenger.negativeRate;


            // CHALLENGER
            info.challengerDifficulty = challenger.difficulty;


            // TODO: add loading screen.
            SceneManager.LoadScene("MatchScene");
        }


        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}