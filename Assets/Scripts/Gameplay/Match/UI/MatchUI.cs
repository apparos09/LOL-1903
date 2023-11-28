using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using util;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace RM_EM
{
    // The UI for matches. All UI elements that call functions should use this script.
    public class MatchUI : GameplayUI
    {
        [Header("Match")]

        // The match manager.
        public MatchManager matchManager;

        // The time for the match UI.
        public TMP_Text timerText;

        // The UI content shown when the match ends.
        public MatchEnd matchEnd;

        [Header("Match/Players")]

        // Player 1's UI
        public PlayerMatchUI p1UI;

        // Player 2's UI
        public PlayerMatchUI p2UI;

        [Header("Match/Value Box")]

        // The value box prefab.
        public GameObject valueBoxPrefab;

        // A pool of value boxes to pull from.
        public Queue<GameObject> valueBoxPool;

        [Header("Match/Buttons")]

        // Button to return to the world.
        public Button worldButton;

        // The replay button.
        // public Button replayButton;

        // The match quit button.
        // public Button matchQuitButton;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the instance.
            if (matchManager == null)
                matchManager = MatchManager.Instance;

            // Hides the match end.
            HideMatchEnd();
        }

        // TUTORIAL

        // On Tutorial Start
        public override void OnTutorialStart()
        {
            base.OnTutorialStart();

            // P1
            p1UI.powerButton.interactable = false;
            p1UI.skipButton.interactable = false;

            // P2
            p2UI.powerButton.interactable = false;
            p2UI.skipButton.interactable = false;

            // Other
            worldButton.interactable = false;
        }

        // On Tutorial End
        public override void OnTutorialEnd()
        {
            base.OnTutorialEnd();

            // P1
            p1UI.powerButton.interactable = p1UI.playerMatch.IsPowerAvailable();
            p1UI.skipButton.interactable = p1UI.playerMatch.CanSkipEquation();

            // P2
            p2UI.powerButton.interactable = p2UI.playerMatch.IsPowerAvailable();
            p2UI.skipButton.interactable = p2UI.playerMatch.CanSkipEquation();

            // Other
            worldButton.interactable = true;
        }

        // INTERFACE UPDATES //

        // Updates the timer displayed.
        public void UpdateTimerText()
        {
            timerText.text = GameplayManager.GetTimeFormatted(matchManager.matchTime);
        }

        // Updates all player displays.
        public void UpdateAllPlayerUI()
        {
            // Seperate into different functions.

            // Player 1
            UpdatePlayer1EquationDisplay();
            UpdatePlayer1PointsBar();
            UpdatePlayer1PowerBarFill();
            UpdatePlayer1PowerBarColor();

            // Player 2
            UpdatePlayer2EquationDisplay();
            UpdatePlayer2PointsBar();
            UpdatePlayer2PowerBarFill();
            UpdatePlayer2PowerBarColor();
        }

        // Updates UI that changes when an equation is compelted.
        public bool UpdateOnEquationCompleteUI(PlayerMatch player)
        {
            // The variable that checks the update.
            bool updated = false;

            // Updates the displays.
            if (player == matchManager.p1)
            {
                UpdatePlayer1EquationDisplay();
                UpdatePlayer1PointsBar();
                UpdatePlayer1PowerBarFill();
                updated = true;
            }
            else if (player == matchManager.p2)
            {
                UpdatePlayer2EquationDisplay();
                UpdatePlayer2PointsBar();
                UpdatePlayer2PowerBarFill();
                updated = true;
            }

            return updated;
        }

        // EQUATION DISPLAYS
        // Updates the player 1 equation.
        public void UpdatePlayer1EquationDisplay()
        {
            p1UI.UpdatePlayerEquationDisplay();
        }

        // Updates the player 2 equation.
        public void UpdatePlayer2EquationDisplay()
        {
            p2UI.UpdatePlayerEquationDisplay();
        }


        // PROGRESS BARS //

        // Updates player 1's points bar.
        public void UpdatePlayer1PointsBar()
        {
            p1UI.UpdatePlayerPointsBar();
        }

        // Updates player 2's points bar.
        public void UpdatePlayer2PointsBar()
        {
            p2UI.UpdatePlayerPointsBar();
        }

        // POWERS //

        // Updates player 1's power bar.
        public void UpdatePlayer1PowerBarFill()
        {
            p1UI.UpdatePlayerPowerBarFill();
        }

        // Update's player 2's power bar.
        public void UpdatePlayer2PowerBarFill()
        {
            p2UI.UpdatePlayerPowerBarFill();
        }

        // Updates player 1's power bar color.
        public void UpdatePlayer1PowerBarColor()
        {
            p1UI.UpdatePlayerPowerBarColor();
        }

        // Updates player 2's power bar color.
        public void UpdatePlayer2PowerBarColor()
        {
            p2UI.UpdatePlayerPowerBarColor();
        }

        // OPERATIONS
        // Use player 1's power.
        public void UsePlayer1Power()
        {
            matchManager.p1.UsePower();
        }

        // Use player 2's power.
        public void UsePlayer2Power()
        {
            matchManager.p2.UsePower();
        }

        // Skips
        // Player 1 Skip
        public void UsePlayer1EquationSkip()
        {
            matchManager.p1.SkipEquation(true);
        }

        // Player 2 Skip
        public void UsePlayer2EquationSkip()
        {
            // P2 always ignores the limit.
            matchManager.p2.SkipEquation(false);
        }


        // Equation Displays
        // Value Box
        // Gets a value box prefab.
        public GameObject GetValueBox()
        {
            GameObject valueBox;

            // Checks if a value is in the pool.
            if (valueBoxPool.Count == 0) // None in pool.
            {
                valueBox = Instantiate(valueBoxPrefab);
            }
            else
            {
                // Grabs from the pool.
                valueBox = valueBoxPool.Dequeue();
            }

            // Set the box to active.
            valueBox.SetActive(true);

            return valueBox;
        }

        // Returns a value box, setting its new parent and position.
        public GameObject GetValueBox(Transform newParent, Vector3 newPos)
        {
            GameObject valueBox = GetValueBox();
            valueBox.transform.parent = newParent;
            valueBox.transform.position = newPos;

            return valueBox;
        }

        // Returns a value box, setting its position.
        public GameObject GetValueBox(Vector3 newPos)
        {
            return GetValueBox(null, newPos);
        }

        // Returns a value box, setting its parent.
        public GameObject GetValueBox(Transform newParent)
        {
            return GetValueBox(newParent, Vector3.zero);
        }

        // Puts a value box back in the pool.
        public void ReturnValueBox(GameObject valueBox)
        {
            // Remove parent and set position to zero.
            valueBox.transform.parent = null;
            valueBox.transform.position = Vector3.zero;

            // Set to inactive.
            valueBox.SetActive(false);

            // Put in pool.
            valueBoxPool.Enqueue(valueBox);
        }


        // WINDOWS //

        // Checks if match end is active.
        public bool IsMatchEndActive()
        {
            return matchEnd.gameObject.activeSelf;
        }

        // Shows the match end.
        public void ShowMatchEnd()
        {
            // Make active.
            matchEnd.gameObject.SetActive(true);

            // Set the player win text.
            matchEnd.SetPlayerWinText();
        }

        // Hides the match end.
        public void HideMatchEnd()
        {
            matchEnd.gameObject.SetActive(false);
        }

        // Reset Match UI - only call this after related values have been reset.
        public void ResetMatchUI()
        {
            // TODO: updating the power bar may not be needed here.
            // Updates P1 UI
            p1UI.UpdatePlayerPointsBar();
            p1UI.UpdatePlayerPowerBarFill();

            // Updates P2 UI
            p2UI.UpdatePlayerPointsBar();
            p2UI.UpdatePlayerPowerBarFill();

            // Close all the windows.
            CloseAllWindows();

            // Hide match end if it's currently active.
            if (IsMatchEndActive())
                HideMatchEnd();

            // Updates the timer text. Time should be set to 0 now.
            UpdateTimerText();
        }


        // Return to the game world.
        public void ToWorldScene()
        {
            matchManager.ToWorldScene();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // The match isn't paused.
            if (!matchManager.MatchPaused)
            {
                // Updates the timer text.
                UpdateTimerText();

                // Enables/disables power buttons

                // P1 Power
                if (matchManager.p1.IsPowerAvailable()) // Power available.
                {
                    if (!p1UI.powerButton.interactable)
                        p1UI.powerButton.interactable = true;
                }
                else // Power not available.
                {
                    if (p1UI.powerButton.interactable)
                        p1UI.powerButton.interactable = false;
                }

                // P2 Power
                if (matchManager.p2.IsPowerAvailable()) // Power available.
                {
                    if (!p2UI.powerButton.interactable)
                        p2UI.powerButton.interactable = true;
                }
                else // Power not available.
                {
                    if (p2UI.powerButton.interactable)
                        p2UI.powerButton.interactable = false;
                }
            }

        }
    }
}