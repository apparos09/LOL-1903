using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The UI for the title scene.
    public class TitleUI : MonoBehaviour
    {
        public TitleManager manager;

        [Header("Buttons")]

        // The new game button and continue button.
        public Button newGameButton;
        public Button continueButton;

        // The controls, settings, and credits.
        public Button controlsButton;
        public Button settingsButton;
        public Button creditsButton;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = TitleManager.Instance;

            // If the LOLSDK isn't initialized, make the continue button non-interactable.
            if(!GameSettings.InitializedLOLSDK)
            {
                continueButton.interactable = false;
            }
        }

        // Starts the new game.
        public void StartNewGame()
        {
            manager.StartNewGame();
        }

        // Continues the game.
        public void ContinueGame()
        {
            manager.ContinueGame();
        }

        // Opens the controls window.
        public void OpenControlsWindow()
        {
            // TODO: implement
        }

        // Opens the settings window.
        public void OpenSettingsWindow()
        {
            // TODO: implement
        }

        // Opens the credits window.
        public void OpenCreditsWindow()
        {
            // TODO: implement
        }


    }
}