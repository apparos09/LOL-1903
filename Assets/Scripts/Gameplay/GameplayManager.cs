using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_EM
{
    // A parent class for managing all gameplay.
    public class GameplayManager : MonoBehaviour
    {
        // The game UI.
        public GameplayUI gameUI;

        // The timer for the game.
        public float gameTime = 0;

        // Pauses the timer if true.
        protected bool gamePaused = false;

        // The mouse touch object.
        public MouseTouchInput mouseTouch;

        // The tutorial.
        public Tutorial tutorial;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Grabs the tutorial instance.
            if (tutorial == null)
                tutorial = Tutorial.Instance;

            // If the gameUI is set, check for the tutorial text box.
            if(gameUI != null)
            {
                // If the tutorial text box is set...
                if(gameUI.tutorialTextBox != null)
                {
                    // Adds the callbakcs from the tutorial text box.
                    // I don't think I need to remove them.
                    gameUI.AddTutorialTextBoxCallbacks(this);
                }
            }
        }

        // Returns the provided time (in seconds), formatted.
        public static string GetTimeFormatted(float seconds, bool roundUp = true)
        {
            // Gets the time and rounds it up to the nearest whole number.
            float time = (roundUp) ? Mathf.Ceil(seconds) : seconds;

            // Formats the time.
            string formatted = StringFormatter.FormatTime(time, false, true, false);

            // Returns the formatted time.
            return formatted;
        }

        // Sets if the game should be paused.
        public virtual void SetPausedGame(bool paused)
        {
            gamePaused = paused;
        }

        // Checks if the game is paused.
        public bool GamePaused
        {
            get
            {
                return gamePaused;
            }
        }

        // Pauses the game.
        public virtual void PauseGame()
        {
            SetPausedGame(true);
        }

        // Unpauses the game.
        public virtual void UnpauseGame()
        {
            SetPausedGame(false);
        }

        // Toggles if the game is paused or not.
        public virtual void TogglePausedGame()
        {
            SetPausedGame(!gamePaused);
        }

        // TUTORIAL //

        // Checks if the game is using the tutorial.
        public bool IsUsingTutorial()
        {
            bool result = GameSettings.Instance.UseTutorial;
            return result;
        }

        // Starts a tutorial using the provided pages.
        public void StartTutorial(List<Page> pages)
        {
            // Sets the pages and opens the text box.
            gameUI.tutorialTextBox.pages = pages;
            gameUI.tutorialTextBox.CurrentPageIndex = 0;
            gameUI.tutorialTextBox.Open();
        }

        // Called when a tutorial is started.
        public virtual void OnTutorialStart()
        {
            // Start...
        }

        // Called when a tutorial is ended.
        public virtual void OnTutorialEnd()
        {
            // End...
        }

        // Called when the game is completed.
        public virtual void OnGameComplete()
        {
            ToResultsScene();
        }

        // SCENES //
        // Go to the title scene.
        public virtual void ToTitleScene()
        {
            // TODO: add loading screen.
            SceneManager.LoadScene("TitleScene");
        }

        // Go to the resultsscene.
        public virtual void ToResultsScene()
        {
            // TODO: add loading screen.
            SceneManager.LoadScene("ResultsScene");
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // The game isn't paused.
            if(!gamePaused)
            {
                gameTime += Time.fixedDeltaTime;
            }
        }
    }
}