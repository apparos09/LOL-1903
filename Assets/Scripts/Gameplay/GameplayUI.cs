using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The game UI.
    public class GameplayUI : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // The settings UI.
        // TODO: add quit button.
        public GameSettingsUI settingsUI;

        // The tutorial text box.
        public TextBox tutorialTextBox;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // TUTORIAL //
        // Start tutorial
        public void StartTutorial(List<Page> pages)
        {
            // Sets the pages and opens the text box.
            tutorialTextBox.pages = pages;
            tutorialTextBox.CurrentPageIndex = 0;
            tutorialTextBox.Open();
        }

        // On Tutorial Start
        public virtual void OnTutorialStart()
        {
            // ...
        }

        // On Tutorial End
        public virtual void OnTutorialEnd()
        {
            // ...
        }

        // Checks if the tutorial text box is open.
        public bool IsTutorialTextBoxOpen()
        {
            return tutorialTextBox.IsVisible();
        }

        // Returns 'true' if the tutorial can be started.
        public bool IsTutorialAvailable()
        {
            return !IsTutorialTextBoxOpen();
        }

        // Adds the tutorial text box open/close callbacks.
        public void AddTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialTextBox.OnTextBoxOpenedAddCallback(manager.OnTutorialStart);
            tutorialTextBox.OnTextBoxClosedAddCallback(manager.OnTutorialEnd);
        }

        // Removes the tutorial text box open/close callbacks.
        public void RemoveTutorialTextBoxCallbacks(GameplayManager manager)
        {
            tutorialTextBox.OnTextBoxOpenedRemoveCallback(manager.OnTutorialStart);
            tutorialTextBox.OnTextBoxClosedRemoveCallback(manager.OnTutorialEnd);
        }

        // WINDOWS //
        // Opens the settings.
        public void OpenSettings()
        {
            // TODO: pause game.
            settingsUI.gameObject.SetActive(true);
            OnWindowOpened(settingsUI.gameObject);
        }

        // Closes the settings.
        public void CloseSettings()
        {
            // Close the settings window.
            settingsUI.gameObject.SetActive(false);
            OnWindowClosed();

            // Close all the windows.
            // CloseAllWindows();
        }

        // Toggles the settings on and off.
        public void ToggleSettings()
        {
            // If the settings is active, close it.
            if(settingsUI.gameObject.activeSelf) 
            {
                CloseSettings();
            }
            else // Open the settings.
            {
                OpenSettings();
            }
        }

        // Closes all the windows.
        public virtual void CloseAllWindows()
        {
            // Settings
            settingsUI.gameObject.SetActive(false);
            
            // On Window Closed
            OnWindowClosed();
        }

        // Called when a window is opened.
        public virtual void OnWindowOpened(GameObject window)
        {
            gameManager.PauseGame();

            // Turns off the settings window if it wasn't the one that got turned on.
            if (window != settingsUI.gameObject)
                settingsUI.gameObject.SetActive(false);
        }

        // Called when a window is closed.
        public virtual void OnWindowClosed()
        {
            // Checks for the tutorial text box.
            if(tutorialTextBox != null)
            {
                // Unpause the game only if the tutorial textbox is closed.
                if(!tutorialTextBox.IsVisible())
                    gameManager.UnpauseGame();

            }
            else // Regular unpause.
            {
                gameManager.UnpauseGame();
            }
            
        }

        // Checks if a window is open.
        public virtual bool IsWindowOpen()
        {
            // Only checks the settings window here.
            bool open = settingsUI.gameObject.activeSelf;

            return open;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}