using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using util;

namespace RM_EM
{
    // The UI for matches.
    public class MatchUI : GameplayUI
    {
        [Header("Match")]

        // The match manager.
        public MatchManager manager;

        // The time for the match UI.
        public TMP_Text timeText;

        // The UI contnet shown when the match ends.
        public GameObject matchEnd;

        [Header("Match/Player 1")]
        // The player 1 equation.
        public TMP_Text p1EquationText;

        // The player 1 points bar.
        public ProgressBar p1PointsBar;

        [Header("Match/Player 2/Computer")]
        // The player 2 equation.
        public TMP_Text p2EquationText;

        // The player 2 points bar.
        public ProgressBar p2PointsBar;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Grabs the instance.
            if (manager == null)
                manager = MatchManager.Instance;

            // Hides the match end.
            HideMatchEnd();
        }


        // Updates the timer displayed.
        public void UpdateTimeText()
        {
            timeText.text = GameplayManager.GetTimeFormatted(manager.matchTime);
        }


        // Updates the player 1 equation.
        public void UpdatePlayer1EquationDisplay()
        {
            // Gets the equation question formatted.
            p1EquationText.text = manager.p1Puzzle.GetEquationQuestionFormatted();
        }

        // Updates the player 2 equation.
        public void UpdatePlayer2EquationDisplay()
        {
            // Gets the equation question formatted.
            p2EquationText.text = manager.p2Puzzle.GetEquationQuestionFormatted();
        }

        // Gets the percentage of the points bar filled.
        private float GetPointsGoalPercentage(float points)
        {
            // The percent to be returned.
            float percent = 0.0F;

            // Checks if the point goal is set, and if it should even be used at all.
            if(manager.usePointGoal)
            {
                if (manager.pointGoal <= 0)
                {
                    percent = 1.0F; // Always set at 100.
                }
                else
                {
                    // Calculates the percentage and clamps it.
                    percent = (points) / manager.pointGoal;
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

        // Updates player 1's points bar.
        public void UpdatePlayer1PointsBar()
        {
            // Percent
            float percent = GetPointsGoalPercentage(manager.p1.points);

            // Sets the points bar percentage.
            p1PointsBar.SetValueAsPercentage(percent);
        }

        // Updates player 2's points bar.
        public void UpdatePlayer2PointsBar()
        {
            // Percent
            float percent = GetPointsGoalPercentage(manager.p2.points);

            // Sets the points bar percentage.
            p2PointsBar.SetValueAsPercentage(percent);
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