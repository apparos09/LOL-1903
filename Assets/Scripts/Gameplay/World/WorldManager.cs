using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        // This isn't needed.
        // // The list for world events.
        // public List<GameEvent> worldEvents = new List<GameEvent>();

        // An object that's used to block world colliders.
        public GameObject colliderBlocker;

        // Used to pause the world.
        protected bool worldPaused = false;

        [Header("Player")]

        // The player in the game world.
        public PlayerWorld playerWorld;

        [Header("Areas, Challengers")]
        // The areas in the world.
        public List<Area> areas = new List<Area>();

        // THe current area index.
        public int currAreaIndex = 0;

        // A list of all challengers in the game.
        public List<ChallengerWorld> challengers;

        // The total number of challengers in the game.
        public const int CHALLENGER_COUNT = 9;

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

            // Turn off the blocker.
            colliderBlocker.SetActive(false);

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

            // The power menu can only be accessed if the player has powers.
            // TODO: once the game is done, you can take out these null checks since this should always be present.
            //if(worldUI.powerMenuUI != null)
            //{
            //    if(worldUI.powerMenuUI.menuButton != null)
            //    {
            //        worldUI.powerMenuUI.menuButton.interactable = playerWorld.HasPowers();
            //    }
            //}

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

            // This is being used for testing purposes. The game settings should be consulted for this value.
            // Checks if the game settings have you use the tutorial.
            bool useTutorial = true;
            // if(GameSettings.Instance.UseTutorial)
            if (useTutorial)
            {
                Tutorial tutorial = Tutorial.Instance;

                // Opening not cleared, and the textbox isn't open.
                if (!tutorial.clearedOpening && !worldUI.tutorialTextBox.IsVisible())
                {
                    // Gets the opening tutorial.
                    StartTutorial(tutorial.GetOpeningTutorial());
                    // Input.
                }
            }
            

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

            // Turn on the blocker.
            colliderBlocker.SetActive(true);
        }

        // Called when a tutorial is ended.
        public override void OnTutorialEnd()
        {
            UnpauseWorld();

            // Turn off the blocker, but only if a window isn't open.
            if(!worldUI.IsWindowOpen())
                colliderBlocker.SetActive(false);
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

        // Returns the match number, which is determined based on...
        // How many challengers are left.
        public int GetMatchNumber()
        {
            // The defeated count.
            int defeatedCount = 0;

            // Goes through every challenger.
            foreach(ChallengerWorld challenger in challengers)
            {
                // If the challenger has been defeated, increase the count.
                if (challenger.defeated)
                    defeatedCount++;
            }

            // Adds +1 so that it works as the match count.
            return defeatedCount + 1;
        }

        // SAVING/LOADING
        // Generates the save data or the game.
        public EM_GameData GenerateSaveData()
        {
            // The data.
            EM_GameData data = new EM_GameData();

            // Save the current area index.
            data.currAreaIndex = currAreaIndex;

            // Saves the player's current power.
            data.playerPower = playerWorld.power;

            // Stores the player's power information.
            for (int i = 0; i < data.playerPowerList.Length && i < playerWorld.powerList.Count; i++)
            {
                data.playerPowerList[i] = playerWorld.powerList[i];
            }

            // Stores information on the challengers that have been defeated.
            for (int i = 0; i < data.challengersDefeated.Length && i < challengers.Count; i++)
            {
                // Saves if the challenger has been defeated.
                // NOTE: you don't check challenger available, but I don't think you need to.
                data.challengersDefeated[i] = challengers[i].defeated;
            }

            // TODO: add tutorial clears.

            // The data is valid.
            data.valid = true;

            // Returns 'true'.
            return data;
        }

        // Saves the data for the game.
        public bool SaveGame()
        {
            // If the LOL Manager does not exist, return false.
            if (!LOLManager.Instantiated)
            {
                Debug.LogError("The LOL Manager does not exist.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSys = LOLManager.Instance.saveSystem;

            // Checks if the save system exists.
            if (saveSys == null)
            {
                Debug.LogError("The save system could not be found.");
                return false;
            }
            

            // Set the world manager.
            if(saveSys.worldManager == null)
                saveSys.worldManager = this;

            // Saves the game.
            bool result = saveSys.SaveGame();
            return result;
        }

        // Loads data, and return a 'bool' to show it was successful.
        public bool LoadGame()
        {
            // If the LOL Manager does not exist, return false.
            if(!LOLManager.Instantiated)
            {
                Debug.LogError("The LOL Manager does not exist.");
                return false;
            }

            // Gets the save system.
            SaveSystem saveSys = LOLManager.Instance.saveSystem;

            // Checks if the save system exists.
            if(saveSys == null)
            {
                Debug.LogError("The save system could not be found.");
                return false;
            }

            // No data to load.
            if(saveSys.loadedData == null)
            {
                Debug.LogError("The save data does not exist.");
                return false;
            }

            // Data invalid.
            if (saveSys.loadedData.valid == false)
            {
                Debug.LogError("The save data is invalid.");
                return false;
            }

            // Gets the loaded data.
            EM_GameData loadedData = saveSys.loadedData;

            // LOADING THE DATA
            // Save the current area index.
            SetArea(loadedData.currAreaIndex);


            // Clears the power list.
            playerWorld.powerList.Clear();

            // Gives the player their powers.
            for (int i = 0; i < loadedData.playerPowerList.Length; i++)
            {
                // Gives the player the power.
                playerWorld.GivePower(loadedData.playerPowerList[i], false);
            }

            // Sets the player's current power.
            playerWorld.SetPower(loadedData.playerPower);

            // Sets if challengers have been defeated or not.
            for (int i = 0; i < loadedData.challengersDefeated.Length && i < challengers.Count; i++)
            {
                // Sets if the challenger has been defeated.
                challengers[i].defeated = loadedData.challengersDefeated[i];
            }

            // TODO: implement tutorial content.

            // The data has been loaded successfully.
            return true;
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


        // GAME COMPLETE
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