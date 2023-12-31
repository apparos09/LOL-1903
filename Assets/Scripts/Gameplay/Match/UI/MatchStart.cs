using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_EM
{
    // The match start script.
    public class MatchStart : MonoBehaviour
    {
        // The manager.
        public MatchManager manager;

        // The match UI.
        public MatchUI matchUI;

        // The animator.
        public Animator animator;

        // The display text.
        public TMP_Text displayText;

        // Start is called before the first frame update
        void Start()
        {
            // Manager
            if (manager == null)
                manager = MatchManager.Instance;

            // Match UI
            if(matchUI == null)
                matchUI = manager.matchUI;
        }

        // Plays the start animation.
        public void PlayStartAnimation()
        {
            animator.Play("Match Start Animation");
        }

        // Called when the start animation is started.
        public void OnStartAnimationStart()
        {
            manager.PauseMatch();
        }

        // Sets the text to ready.
        public void SetTextToReady()
        {
            // TODO: use language file translation
            displayText.text = "Ready...";
        }

        public void SetTextToStart()
        {
            // TODO: use language file translation
            displayText.text = "Start!";
        }

        // Called when the start animation is ended.
        public void OnStartAnimationEnd()
        {
            // Unpauses the match.
            manager.UnpauseMatch();

            // Plays the empty animation.
            PlayEmptyAnimation();
        }

        // Plays the empty animation.
        public void PlayEmptyAnimation()
        {
            // Plays the empty animation to reset the animation trigger.
            if (animator != null)
                animator.Play("Empty");
        }

    }
}