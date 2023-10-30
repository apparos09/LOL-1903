using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using util;

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

        // The quit button.
        public Button quitButton;

        [Header("Windows")]
        // The title window.
        public GameObject titleWindow;

        // The controls, settings, and credits windows.
        public GameObject controlsWindow;
        public GameSettingsUI settingsWindow;
        public AudioCreditsInterface creditsWindow;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = TitleManager.Instance;

            // If the platform is set to webGL, disable the quit button.
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                quitButton.interactable = false;
            }

            // If the LOLSDK isn't initialized, make the continue button non-interactable.
            if (!GameSettings.InitializedLOLSDK)
            {
                // Disable continue.
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


        // OPENING/CLOSING WINDOWS
        
        // Title - Open
        public void OpenTitleWindow()
        {
            CloseAllSubWindows();
            titleWindow.SetActive(true);
        }

        // Title - Close
        public void CloseTitleWindow()
        {
            titleWindow.SetActive(false);
        }

        // Controls - Open
        public void OpenControlsWindow()
        {
            // Close all sub windows, and the title window.
            CloseAllSubWindows();
            CloseTitleWindow();

            // Open the controls window.
            controlsWindow.gameObject.SetActive(true);
        }

        // Controls - Close
        public void CloseControlsWindow()
        {
            CloseAllSubWindows();
        }

        // Settings - Open
        public void OpenSettingsWindow()
        {
            // Close all sub windows, and the title window.
            CloseAllSubWindows();
            CloseTitleWindow();

            // Open the settings window.
            settingsWindow.gameObject.SetActive(true);
        }

        // Settings - Close
        public void CloseSettingsWindow()
        {
            CloseAllSubWindows();
        }

        // Credits - Open
        public void OpenCreditsWindow()
        {
            // Close all sub windows, and the title window.
            CloseAllSubWindows();
            CloseTitleWindow();

            // Open the credits window.
            creditsWindow.gameObject.SetActive(true);
        }

        // Credits - Close
        public void CloseCreditsWindow()
        {
            CloseAllSubWindows();
        }

        // Closes all sub windows.
        public void CloseAllSubWindows()
        {
            // Close all windows.
            titleWindow.gameObject.SetActive(false);
            controlsWindow.gameObject.SetActive(false);
            settingsWindow.gameObject.SetActive(false);
            creditsWindow.gameObject.SetActive(false);

            // Open the title window.
            titleWindow.SetActive(true);
        }


        // Other
        // Quits the game.
        public void QuitGame()
        {
            manager.QuitGame();
        }
    }
}