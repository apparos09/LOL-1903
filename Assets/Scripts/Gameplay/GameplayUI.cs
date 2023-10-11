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
        public GameSettingsUI settingsUI;

        // The tutorial text box.
        public TextBox tutorialTextBox;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // WINDOWS //
        // Opens the settings.
        public void OpenSettings()
        {
            // TODO: pause game.
            settingsUI.gameObject.SetActive(true);
            OnWindowOpened();
        }

        // Closes the settings.
        public void CloseSettings()
        {
            settingsUI.gameObject.SetActive(false);
            OnWindowClosed();
        }

        // Called when a window is opened.
        public virtual void OnWindowOpened()
        {
            gameManager.PauseGame();
        }

        // Called when a window is closed.
        public virtual void OnWindowClosed()
        {
            gameManager.UnpauseGame();
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
    }
}