using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The game UI.
    public class GameplayUI : MonoBehaviour
    {
        // The settings UI.
        public GameSettingsUI settingsUI;

        // The tutorial text box.
        public TextBox tutorialTextBox;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
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