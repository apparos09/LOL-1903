using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_EM
{
    // The UI for matches.
    public class MatchUI : MonoBehaviour
    {
        // The match manager.
        public MatchManager manager;

        // The time for the match UI.
        public TMP_Text timeText;

        [Header("Player 1")]
        // The player 1 equation.
        public TMP_Text p1EquationText;

        // 

        [Header("Player 2/Computer")]
        // The player 2 equation.
        public TMP_Text p2EquationText;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance.
            if (manager == null)
                manager = MatchManager.Instance;
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}