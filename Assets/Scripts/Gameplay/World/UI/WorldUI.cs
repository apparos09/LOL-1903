using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The World UI. All UI elements in the world that call functions should do so through this script.
    public class WorldUI : GameplayUI
    {
        [Header("World")]

        // The match manager.
        public WorldManager worldManager;

        // The power menu UI.
        public PowerMenuUI powerMenuUI;

        // The save window.
        public GameObject saveWindow;

        // The challenge window.
        public ChallengeUI challengeUI;

        [Header("World/Area")]
        // Button for left room.
        public Button prevAreaButton;

        // Button for going to right room.
        public Button nextAreaButton;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (worldManager == null)
                worldManager = WorldManager.Instance;
        }

        // SETTINGS (EXPANDED) //

        // Opens the power menu.
        public void OpenPowersMenu()
        {
            CloseAllWindows();
            powerMenuUI.gameObject.SetActive(true);
            OnWindowOpened(powerMenuUI.gameObject);
        }

        // Closes the power menu.
        public void ClosePowersMenu()
        {
            powerMenuUI.gameObject.SetActive(false);
            OnWindowClosed();
        }

        // Toggles the powers menu on/off.
        public void TogglePowersMenu()
        {
            // If the powers menu is active, close it.
            if (settingsUI.gameObject.activeSelf)
            {
                ClosePowersMenu();
            }
            else // Open the powers menu.
            {
                OpenPowersMenu();
            }
        }

        // Open the save window.
        public void OpenSaveWindow()
        {
            saveWindow.gameObject.SetActive(true);
            OnWindowOpened(saveWindow);
        }

        // Close the save window.
        public void CloseSaveWindow()
        {
            saveWindow.gameObject.SetActive(false);
            OnWindowClosed();
        }

        // Toggles the save window.
        public void ToggleSaveWindow()
        {
            // If the save window is active, close it.
            if (settingsUI.gameObject.activeSelf)
            {
                CloseSaveWindow();
            }
            else // Open the save window.
            {
                OpenSaveWindow();
            }
        }

        // Saves the game and continues it.
        public void SaveAndContinue()
        {
            worldManager.SaveGame();
            CloseSaveWindow();
        }

        // Save and quit.
        public void SaveAndQuit()
        {
            worldManager.SaveGame();
            worldManager.ToTitleScene();
        }

        // Overrides the on window opened function.
        public override void OnWindowOpened(GameObject window)
        {
            base.OnWindowOpened(window);

            // Turn off the powers window.
            if (window != powerMenuUI.gameObject)
                powerMenuUI.gameObject.SetActive(false);

            // Turns off the save window.
            if (window != saveWindow)
                saveWindow.gameObject.SetActive(false);

            // Turn off the settings window.
            if(window != settingsUI.gameObject)
                settingsUI.gameObject.SetActive(false);

            // Turn on the collider blocker.
            worldManager.colliderBlocker.SetActive(true);
        }

        // Called when a window is closed.
        public override void OnWindowClosed()
        {
            base.OnWindowClosed();

            // Turn off the blocker.
            worldManager.colliderBlocker.SetActive(false);
        }



        // NEXT AREA/PREVIOUS AREA
        // Goes to the next area.
        public void NextArea()
        {
            worldManager.NextArea();
        }

        // Goes to the previous area.
        public void PreviousArea()
        {
            worldManager.PreviousArea();
        }


        // CHALLENGE UI //

        // Sets the challenger UI to be active. If it's being deactivated, the challenger can just be set to null.
        public void SetChallengeUIActive(bool active, ChallengerWorld challenger, int index)
        {
            // Checks if active or inactive.
            if(active)
            {
                challengeUI.SetChallenger(challenger, index);
                challengeUI.gameObject.SetActive(true);
            }
            else
            {
                challengeUI.SetChallenger(null, -1);
                challengeUI.gameObject.SetActive(false);
            }
        }

        // Checks if the challenger UI is active
        public bool IsChallengerUIActive()
        {
            bool result = challengeUI.isActiveAndEnabled;
            return result;
        }

        // Shows the challenge UI.
        public void ShowChallengeUI(ChallengerWorld challenger, int index)
        {
            SetChallengeUIActive(true, challenger, index);
        }

        // Hides the challenge UI.
        public void HideChallengeUI()
        {
            SetChallengeUIActive(false, null, -1);
        }

    }
}