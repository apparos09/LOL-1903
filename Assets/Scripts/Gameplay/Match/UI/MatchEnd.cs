using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The match end.
    public class MatchEnd : MonoBehaviour
    {
        // The match manager.
        public MatchManager matchManager;

        // The match UI.
        public MatchUI matchUI;

        // The match over text.
        public TMP_Text matchOverText;

        // The match over text.
        public TMP_Text playerWinText;

        [Header("Buttons")]
        // The world button.
        public Button worldButton;

        // The replay button.
        public Button rematchButton;

        // The quit button.
        public Button quitButton;

        // Start is called before the first frame update
        void Start()
        {
            SetManagerAndUI();
        }

        // Sets the manager and the UI.
        private void SetManagerAndUI()
        {
            // Set Manager
            if (matchManager == null)
                matchManager = MatchManager.Instance;

            // Set UI
            if (matchUI == null)
                matchUI = matchManager.matchUI;
        }

        // Sets the player win text.
        public void SetPlayerWinText()
        {
            SetManagerAndUI();

            // Checks for defs
            JSONNode defs = SharedState.LanguageDefs;

            // Checks who has won.
            if (matchManager.HasPlayer1Won()) // P1
            {
                playerWinText.text = (defs != null) ? defs["mth_userWins"] : "You Won!";
            }
            else if (matchManager.HasPlayer2Won()) // P2
            {
                playerWinText.text = (defs != null) ? defs["mth_oppWins"] : "The Opponent Won!";
            }
            else // None
            {
                playerWinText.text = "-";
            }
        }

        // Return to the game world.
        public void ToWorldScene()
        {
            SetManagerAndUI();

            matchUI.ToWorldScene();
        }

        // Replays the match.
        public void ReplayMatch()
        {
            SetManagerAndUI();

            matchUI.ReplayMatch();
        }

        // Quits the game.
        public void QuitGame()
        {
            SetManagerAndUI();

            matchUI.QuitGame();
        }

    }
}