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
        // The timer for the game.
        public float gameTime = 0;

        // Pauses the timer if true.
        public bool gamePaused = false;

        // The mouse touch object.
        public MouseTouchInput mouseTouch;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
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

        // SCENES //
        // Go to the title scene.
        public virtual void ToTitleScene()
        {
            // TODO: add loading screen.
            SceneManager.LoadScene("TitleScene");
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