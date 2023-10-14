using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using util;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

namespace RM_EM
{
    // The UI for matches.
    public class MatchUI : GameplayUI
    {
        [Header("Match")]

        // The match manager.
        public MatchManager matchManager;

        // The time for the match UI.
        public TMP_Text timeText;

        // The UI contnet shown when the match ends.
        public GameObject matchEnd;

        [Header("Match/Player 1")]
        // The player 1 equation.
        public TMP_Text p1EquationText;

        // The player 1 match points bar.
        public ProgressBar p1PointsBar;

        // The player 2 power bar.
        public ProgressBar p1PowerBar;

        // The fill image for the player 1 power bar.
        public Image p1PowerBarFill;

        [Header("Match/Player 2/Computer")]
        // The player 2 equation.
        public TMP_Text p2EquationText;

        // The player 2 match points bar.
        public ProgressBar p2PointsBar;

        // The player 2 power bar.
        public ProgressBar p2PowerBar;

        // The fill image for the player 2 power bar.
        public Image p2PowerBarFill;

        [Header("Other")]
        // The power bar color when the player has a power.
        public Color hasPowerColor = Color.red;

        // The power bar color when the player has no power.
        public Color noPowerBarColor = Color.grey;

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


        // Updates the timer displayed.
        public void UpdateTimeText()
        {
            timeText.text = GameplayManager.GetTimeFormatted(matchManager.matchTime);
        }

        // Updates all player displays.
        public void UpdateAllPlayerDisplays()
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

        // EQUATION DISPLAYS
        // Updates the player 1 equation.
        public void UpdatePlayer1EquationDisplay()
        {
            // Gets the equation question formatted.
            p1EquationText.text = matchManager.p1Puzzle.GetEquationQuestionFormatted();
        }

        // Updates the player 2 equation.
        public void UpdatePlayer2EquationDisplay()
        {
            // Gets the equation question formatted.
            p2EquationText.text = matchManager.p2Puzzle.GetEquationQuestionFormatted();
        }


        // PROGRESS BARS //
        // POINTS BARS
        // Gets the percentage of the points bar filled.
        private float GetPointsGoalPercentage(float points)
        {
            // The percent to be returned.
            float percent = 0.0F;

            // Checks if the point goal is set, and if it should even be used at all.
            if(matchManager.usePointGoal)
            {
                if (matchManager.pointGoal <= 0)
                {
                    percent = 1.0F; // Always set at 100.
                }
                else
                {
                    // Calculates the percentage and clamps it.
                    percent = (points) / matchManager.pointGoal;
                    percent = Mathf.Clamp01(percent);
                }
            }
            else
            {
                // Set to max.
                percent = 1.0F;
            }
            

            return percent;
        }

        // Updates the player points bar.
        private void UpdatePlayerPointsBar(PlayerMatch player, ProgressBar bar)
        {
            // Percent
            float percent = GetPointsGoalPercentage(player.points);

            // Sets the points bar percentage.
            bar.SetValueAsPercentage(percent);
        }

        // Updates player 1's points bar.
        public void UpdatePlayer1PointsBar()
        {
            // Updates P1's points bar using this function.
            UpdatePlayerPointsBar(matchManager.p1, p1PointsBar);
        }

        // Updates player 2's points bar.
        public void UpdatePlayer2PointsBar()
        {
            // Updates P2's points bar using this function.
            UpdatePlayerPointsBar(matchManager.p2, p2PointsBar);
        }

        // POWERS //

        // Updates the player's power bar.
        public void UpdatePlayerPowerBarFill(PlayerMatch player, ProgressBar bar)
        {
            // Checks if a player has a power.
            bool hasPower;

            // Percent
            float percent;

            // Sees if the player has a power.
            hasPower = player.HasPower();

            // Does the player have a power?
            if (hasPower) // Get percentage.
            {
                percent = player.power.GetPowerFillPercentage();
            }
            else // Set to full (bar should be greyed out).
            {
                percent = 1.0F;
            }

            // Sets the points bar percentage.
            bar.SetValueAsPercentage(percent);
        }

        // Updates player 1's power bar.
        public void UpdatePlayer1PowerBarFill()
        {
            UpdatePlayerPowerBarFill(matchManager.p1, p1PowerBar);
        }

        // Update's player 2's power bar.
        public void UpdatePlayer2PowerBarFill()
        {
            UpdatePlayerPowerBarFill(matchManager.p2, p2PowerBar);
        }

        // Updates the player power bar color.
        private void UpdatePlayerPowerBarColor(PlayerMatch player, ProgressBar bar, Image fillImage)
        {
            // Checks if the player has a power.
            bool hasPower = player.HasPower();

            // Fills the image.
            fillImage.color = (hasPower) ? hasPowerColor : noPowerBarColor;

            // If the player has no power, set the bar to be full. 
            if (!hasPower)
                bar.SetValueAsPercentage(1.0F, false);
        }

        // Updates player 1's power bar color.
        public void UpdatePlayer1PowerBarColor()
        {
            UpdatePlayerPowerBarColor(matchManager.p1, p1PowerBar, p1PowerBarFill);
        }

        // Updates player 2's power bar color.
        public void UpdatePlayer2PowerBarColor()
        {
            UpdatePlayerPowerBarColor(matchManager.p2, p2PowerBar, p2PowerBarFill);
        }


        // Shows the match end.
        public void ShowMatchEnd()
        {
            matchEnd.SetActive(true);
        }

        // Hides the match end.
        public void HideMatchEnd()
        {
            matchEnd.SetActive(false);
        }

        
    }
}