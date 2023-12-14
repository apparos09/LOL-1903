using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The UI for the challenger.
    public class ChallengerUI : MonoBehaviour
    {
        // The world manager.
        public WorldManager manager;

        // The challenger the challenge is being issued by.
        public ChallengerWorld challenger;

        // The index of the challenger.
        public int challengerIndex = -1;

        // The renderer of the challenger art.
        public Image challengerRenderer;

        [Header("Text")]

        // The name text.
        public TMP_Text nameText;

        // The species text.
        public TMP_Text speciesText;

        // The quote text.
        public TMP_Text quoteText;

        // The rules text for the UI.
        public TMP_Text rulesText;

        [Header("Buttons")]

        // Accept Button
        public Button acceptButton;

        // Decline Button
        public Button declineButton;

        // Start is called before the first frame update
        void Start()
        {
            // Manager set.
            if (manager == null)
                manager = WorldManager.Instance;
        }

        // May not be needed.
        //// This function is called when the object becomes enabled and active
        //private void OnEnable()
        //{
        //    EnableButtons();
        //}

        // Sets the challenger.
        public void SetChallenger(ChallengerWorld newChallenger, int index)
        {
            challenger = newChallenger;
            challengerIndex = index;

            UpdateChallengerSprite();
            UpdateNameText();
            UpdateSpeciesText();
            UpdateQuoteText();
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

        // Updates the name text.
        public void UpdateNameText()
        {
            // Checks if the challenger is set.
            if (challenger != null)
                nameText.text = challenger.challengerName;
            else
                nameText.text = "-";
        }

        // Updates the species text.
        public void UpdateSpeciesText()
        {
            // Checks if the challenger is set.
            if (challenger != null)
                speciesText.text = challenger.challengerSpecies;
            else
                speciesText.text = "-";
        }

        // Updates the quote text.
        public void UpdateQuoteText()
        {
            // Checks if the challenger is set.
            if (challenger != null)
                quoteText.text = "\"" + challenger.challengerQuote + "\"";
            else
                quoteText.text = "\"-\"";
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

            // The rule count.
            const int RULE_COUNT_MAX = 6;

            // Exponent Rule
            if (challenger.exponentRate > 0)
                ruleNames.Add(PuzzleInfo.GetRuleName(exponentRule.exponent));

            // Product Rule
            if (challenger.productRate> 0)
                ruleNames.Add(PuzzleInfo.GetRuleName(exponentRule.product));

            // Power of a Power
            if (challenger.powerOfAPowerRate > 0)
                ruleNames.Add(PuzzleInfo.GetRuleName(exponentRule.powerOfAPower));

            // Power of a Product
            if (challenger.powerOfAProductRate > 0)
                ruleNames.Add(PuzzleInfo.GetRuleName(exponentRule.powerOfAProduct));

            // Zero Rule
            if (challenger.zeroRate > 0)
                ruleNames.Add(PuzzleInfo.GetRuleName(exponentRule.zero));

            // Negative Rule
            if (challenger.negativeRate > 0)
                ruleNames.Add(PuzzleInfo.GetRuleName(exponentRule.negative));


            // Checks for valid rules.
            if(ruleNames.Count != 0)
            {
                // Set to rule count max, so just list as "ALL"
                if(ruleNames.Count == RULE_COUNT_MAX)
                {
                    // Checks if the LOLSDK is initialized.
                    if(LOLManager.IsLOLSDKInitialized())
                    {
                        // Defs
                        JSONNode defs = SharedState.LanguageDefs;

                        // Set to 'All'.
                        rules = defs["kwd_all"];
                    }
                    else
                    {
                        // Set to 'All'.
                        rules = "All";
                    }
                }
                else // Add the rule names.
                {
                    // Adds all the rules to the string.
                    for (int i = 0; i < ruleNames.Count; i++)
                    {
                        // Add the rule.
                        rules += ruleNames[i];

                        // If this isn't the last rule, add a slash.
                        if (i + 1 < ruleNames.Count)
                        {
                            rules += "/";
                        }
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

        // Enables the buttons for the challenger UI.
        public void EnableButtons()
        {
            acceptButton.interactable = true;
            declineButton.interactable = true;
        }

        // Disables the buttons for the challenger UI.
        public void DisableButtons()
        {
            acceptButton.interactable = false;
            declineButton.interactable = false;
        }
    }
}