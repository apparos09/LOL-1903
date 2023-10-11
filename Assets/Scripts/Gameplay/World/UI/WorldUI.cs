using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The World UI
    public class WorldUI : GameplayUI
    {
        [Header("World")]

        // The match manager.
        public WorldManager worldManager;

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

        // CHALLENGE UI //

        // Sets the challenger UI to be active. If it's being deactivated, the challenger can just be set to null.
        public void SetChallengeUIActive(bool active, ChallengerWorld challenger)
        {
            // Checks if active or inactive.
            if(active)
            {
                challengeUI.challenger = challenger;
                challengeUI.gameObject.SetActive(true);
            }
            else
            {
                challengeUI.challenger = null;
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
        public void ShowChallengeUI(ChallengerWorld challenger)
        {
            SetChallengeUIActive(true, challenger);
        }

        // Hides the challenge UI.
        public void HideChallengeUI()
        {
            SetChallengeUIActive(false, null);
        }

    }
}