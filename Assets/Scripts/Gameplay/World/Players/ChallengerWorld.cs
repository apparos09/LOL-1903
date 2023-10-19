using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // A challenger that's encountered in the game world.
    // The 'World' part is added to make it clearer that it belongs to the world area, not the match area.
    public class ChallengerWorld : MonoBehaviour
    {
        // World manager.
        public WorldManager manager;

        // The collider for the challenger.
        public new BoxCollider2D collider;

        // The sprite renderer for the the challenger.
        public SpriteRenderer spriteRenderer;

        // The sprite for the challenger prompt.
        [Tooltip("The sprite for the challenge prompt.")]
        public Sprite challengeSprite;

        [Header("Info")]

        // The challenger's name.
        public string challengerName = string.Empty;

        // The challenger's quote.
        public string challengerQuote = string.Empty;

        // The difficulty of the challenger.
        public int difficulty = 0;

        // Sets if the challenger is available for a match.
        public bool available = true;

        // Gets set to 'true' when the challenger has been defeated.
        public bool defeated = false;

        [Header("Puzzle")]
        // The puzzle type.
        public Puzzle.puzzleType puzzle;

        [Header("Exponents")]
        // The exponent rates.
        public float baseExpoRate = 1.0F;

        // Rate for product rule/multiplicaton (same bases) exponents.
        public float multSameRate = 1.0F;

        // Rate for power of a power/exponent by exponent exponents.
        public float expoByExpoRate = 1.0F;

        // Rate for power of a product/multplication (different bases) exponents.
        public float multDiffRate = 1.0F;

        // Rate for zero exponent rule.
        public float zeroRate = 1.0F;

        // Rate for negative exponent rule.
        public float negativeRate = 1.0F;


        // Start is called before the first frame update
        void Start()
        {
            // Manager.
            if (manager == null)
                manager = WorldManager.Instance;

            // Checks for the collider.
            if (collider == null)
            {
                // Tries to get the collider (no longer checks children for misinput concerns).
                collider = GetComponent<BoxCollider2D>();
            }

            // Checks for the sprite renderer.
            if (spriteRenderer == null)
            {
                // Tries to get the component (no longer checks children for misinput concerns).
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        // MouseDown
        private void OnMouseDown()
        {
            // If the challenger UI isn't open, open the UI and set this as the challenger.
            if(!manager.worldUI.IsChallengerUIActive())
            {
                manager.worldUI.ShowChallengeUI(this, manager.GetChallengerIndex(this));
            }
        }

        // Sets if the challenger is locked or not.
        public void SetChallengerAvailable(bool avail)
        {
            available = avail;

            // Checks if the challenger is available.
            if (available) // Available
            {
                collider.enabled = true;
                spriteRenderer.enabled = true;
            }
            else // Unavailable.
            {
                collider.enabled = false;
                spriteRenderer.enabled = false;
            }
        }

        // Sets the challenger to be available.
        public void SetChallengerToAvailable()
        {
            SetChallengerAvailable(true);
        }

        // Sets the challenger to be unavailable.
        public void SetChallengerToUnavailable()
        {
            SetChallengerAvailable(false);
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}