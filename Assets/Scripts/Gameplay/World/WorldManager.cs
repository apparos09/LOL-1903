using JetBrains.Annotations;
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
        private static bool instanced = false;

        // Gets set to 'true', when post start has been called.
        private bool calledPostStart = false;

        [Header("World")]
        // The world UI.
        public WorldUI worldUI;

        // The camera the world uses.
        public WorldCamera worldCamera;

        // Used to pause the world.
        protected bool worldPaused = false;

        // The manager for world events.
        public GameEventManager worldEvents;

        [Header("Areas, Challengers")]
        // The areas in the world.
        public List<Area> areas = new List<Area>();

        // THe current area index.
        public int currAreaIndex = 0;

        // A list of all challengers in the game.
        public List<ChallengerWorld> challengers;

        // The final challenger of the game.
        public ChallengerWorld finalChallenger;

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
            // Checks if the game info has been initialized already.
            bool gameInfoInit = GameplayInfo.Instantiated;

            // Calls the base function.
            base.Start();

            // If the challenger list isn't set, find all challengers.
            if(challengers.Count == 0)
            {
                // Gets the array.
                ChallengerWorld[] arr = FindObjectsOfType<ChallengerWorld>(true);
                challengers = new List<ChallengerWorld>(arr);
            }

            // Hide the challenger UI.
            worldUI.HideChallengeUI();

            // Sets the current area.
            SetArea(currAreaIndex);

            // Checks if the info object has been instantiated to load content from it.
            if (gameInfoInit && GameplayInfo.Instantiated)
            {
                // Gets the instance.
                GameplayInfo gameInfo = GameplayInfo.Instance;

                // Load the world info.
                gameInfo.LoadWorldInfo(this);
            }

            // StartTutorial(tutorial.GetTestPages());
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
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Post start function.
        private void PostStart()
        {
            // Check if the game is over.
            

            // Called post start.
            calledPostStart = true;
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

        // Checks if the world is paused.
        public bool WorldPaused
        {
            get
            {
                return worldPaused;
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


        // TUTORIAL //
        // Called when a tutorial is started.
        public override void OnTutorialStart()
        {
            PauseWorld();
        }

        // Called when a tutorial is ended.
        public override void OnTutorialEnd()
        {
            UnpauseWorld();
        }


        // AREAS //
        // Sets the area.
        public void SetArea(int newIndex)
        {
            // Bounds check to see if the new index is valid.
            if(newIndex >= 0 && newIndex < areas.Count)
            {
                currAreaIndex = newIndex;
            }
            else
            {
                return;
            }

            // Grabs the new area.
            Area newArea = areas[currAreaIndex];

            // Transition the camera.
            if(newArea.cameraPos != null)
            {
                // Move the camera.
                worldCamera.Move(newArea.cameraPos);
            }


            // Enable the buttons by default (will be disabled if unsable in event update).
            worldUI.prevAreaButton.interactable = true;
            worldUI.nextAreaButton.interactable = true;

            // If the area index is set to 0, disable the back button.
            if (currAreaIndex == 0)
            {
                worldUI.prevAreaButton.interactable = false;
            }

            // If the area index is at its max, disable the forward button.
            if(currAreaIndex == areas.Count - 1)
            {
                worldUI.nextAreaButton.interactable = false;
            }

            // If the next button is interactable, check if the area has been cleared.
            if(worldUI.nextAreaButton.interactable)
            {
                // If the area event is set.
                if(newArea.areaEvent != null)
                {
                    // If the area has not been cleared, disable the next button.
                    if(!newArea.areaEvent.cleared)
                    {
                        worldUI.nextAreaButton.interactable = false;

                    }
                }
            }

        }

        // Go to the next area.
        public void NextArea()
        {
            // Reduce index.
            int index = currAreaIndex + 1;

            // Bounds check.
            if(index >= areas.Count)
                index = 0;

            // Set the area index.
            SetArea(index);
        }
        
        // Go to the previous area.
        public void PreviousArea()
        {
            // Reduce index.
            int index = currAreaIndex - 1;

            // Bounds check.
            if (index < 0)
                index = areas.Count - 1;

            // Set the area index.
            SetArea(index);
        }

        // Returns the index of the challenger.
        public int GetChallengerIndex(ChallengerWorld challenger)
        {
            // Sees if the challenger's in the list.
            if(challengers.Contains(challenger))
            {
                return challengers.IndexOf(challenger);
            }
            else
            {
                return -1;
            }
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

        // Called when the game is completed.
        public override void OnGameComplete()
        {
            // Creates the results data.
            GameObject temp = new GameObject("Results Data");
            ResultsData data = temp.AddComponent<ResultsData>();
            DontDestroyOnLoad(temp);

            // TODO: set data values.
            data.gameTime = gameTime;

            // Checks if the gameplay info has been instantiated.
            if(GameplayInfo.Instantiated)
            {
                // Grabs the instance.
                GameplayInfo gameInfo = GameplayInfo.Instance;

                // Sets the wrong answers.
                data.wrongAnswers = gameInfo.wrongAnswers;
            }
            

            // Goes to the scene.
            base.OnGameComplete();
        }

        // SCENES //
        // Goes to the match scene. Call AcceptChallenge() if a match info object should be created.
        public void ToMatchScene()
        {
            // Gets the game info.
            GameplayInfo gameInfo = GameplayInfo.Instance;

            // Save the world and match info to the game info instance.
            gameInfo.SaveWorldInfo(this);
            gameInfo.SaveMatchInfo(this);

            // TODO: add loading screen.
            SceneManager.LoadScene("MatchScene");
        }


        // Update is called once per frame
        protected override void Update()
        {
            // Calls post start if it hasn't been called yet.
            if (!calledPostStart)
                PostStart();

            // Calls the base update function.
            base.Update();
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