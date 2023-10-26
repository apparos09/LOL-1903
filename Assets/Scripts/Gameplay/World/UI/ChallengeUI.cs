using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The UI for the challenger.
    public class ChallengeUI : MonoBehaviour
    {
        // The world manager.
        public WorldManager manager;

        // The challenger the challenge is being issued by.
        public ChallengerWorld challenger;

        // The index of the challenger.
        public int challengerIndex = -1;

        // The renderer of the challenger art.
        public Image challengerRenderer;

        // The rules text for the UI.
        public TMP_Text rulesText;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = WorldManager.Instance;
        }

        // Sets the challenger.
        public void SetChallenger(ChallengerWorld newChallenger, int index)
        {
            challenger = newChallenger;
            challengerIndex = index;

            UpdateChallengerSprite();
            UpdateRulesText();
        }

        // Updates the Challenger Sprite
        public void UpdateChallengerSprite()
        {
            if(challenger != null) // Set sprite.
            {
                challengerRenderer.sprite = challenger.challengeSprite;
            }
            else // Clear sprite.
            {
                challengerRenderer.sprite = null;
            }
            
        }

        // Updates the rules text.
        public void UpdateRulesText()
        {
            // If the challenger isn't set, clear the rules and return.
            if(challenger == null)
            {
                rulesText.text = string.Empty;
                return;
            }

            // Rule names list.
            List<string> ruleNames = new List<string>();

            // Contains a list of all rules.
            string rules = "";

            // Exponent Rule
            if (challenger.baseExpoRate > 0)
                ruleNames.Add(Puzzle.GetRuleName(exponentRule.exponent));

            // Product Rule
            if (challenger.multSameRate> 0)
                ruleNames.Add(Puzzle.GetRuleName(exponentRule.product));

            // Power of a Power
            if (challenger.expoByExpoRate > 0)
                ruleNames.Add(Puzzle.GetRuleName(exponentRule.powerPower));

            // Power of a Product
            if (challenger.multDiffRate > 0)
                ruleNames.Add(Puzzle.GetRuleName(exponentRule.powerProduct));

            // Zero Rule
            if (challenger.zeroRate > 0)
                ruleNames.Add(Puzzle.GetRuleName(exponentRule.zero));

            // Negative Rule
            if (challenger.negativeRate > 0)
                ruleNames.Add(Puzzle.GetRuleName(exponentRule.negative));


            // Checks for valid rules.
            if(ruleNames.Count != 0)
            {
                // Adds all the rules to the string.
                for(int i = 0; i < ruleNames.Count; i++)
                {
                    // Add the rule.
                    rules += ruleNames[i];

                    // If this isn't the last rule, add a slash.
                    if(i + 1 < ruleNames.Count)
                    {
                        rules += "/";
                    }
                }
            }
            else
            {
                rules = "-";
            }

            // Set the rules text.
            rulesText.text = rules;
        }

        // Accepts the challenge.
        public void AcceptChallenge()
        {
            manager.AcceptChallenge();
        }

        // Declines the challenge.
        public void DeclineChallenge()
        {
            manager.DeclineChallenge();
        }
    }
}