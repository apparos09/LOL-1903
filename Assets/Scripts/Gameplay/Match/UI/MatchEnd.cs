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

        // The world button.
        public Button worldButton;

        // Start is called before the first frame update
        void Start()
        {
            // Set Manager
            if(matchManager != null)
                matchManager = MatchManager.Instance;

            // Set UI
            if (matchUI != null)
                matchUI = matchManager.matchUI;
        }

        // Sets the player win text.
        public void SetPlayerWinText()
        {
            // Checks for defs
            JSONNode defs = SharedState.LanguageDefs;

            // If instance isn't set, set it.
            if (matchManager == null)
                matchManager = MatchManager.Instance;

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

    }
}