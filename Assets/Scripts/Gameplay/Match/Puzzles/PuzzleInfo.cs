using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // An instance that holds prefabs for all puzzle types.
    public class PuzzleInfo : MonoBehaviour
    {
        // The singleton instance.
        private static PuzzleInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        [Header("Prefabs")]

        // The keypad mechanic.
        public KeypadMechanic keypad;

        // The bubbles mechanic.
        public BubbleMechanic bubble;

        // The sliding mechanic.
        public SlidingMechanic sliding;

        // The pinball mechanic.
        public PinballMechanic pinball;

        // Constructor
        private PuzzleInfo()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Gets the instance.
        public static PuzzleInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<PuzzleInfo>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Puzzle Prefabs (singleton)");
                        instance = go.AddComponent<PuzzleInfo>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Gets the rule name.
        public static string GetRuleName(exponentRule rule)
        {
            // Sets the rule name.
            string ruleName = "";

            // Translation.
            JSONNode defs = (LOLManager.IsLOLSDKInitialized()) ? SharedState.LanguageDefs : null;

            // Checks the rule.
            switch (rule)
            {
                default:
                case exponentRule.none:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "None";
                    break;

                case exponentRule.exponent:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "Exponent";
                    break;

                case exponentRule.product:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "Product";
                    break;

                case exponentRule.powerOfAPower:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "Power of a Power";
                    break;

                case exponentRule.powerOfAProduct:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "Power of a Product";
                    break;

                case exponentRule.zero:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "Zero";
                    break;

                case exponentRule.negative:
                    ruleName = (defs != null) ? defs[GetRuleNameSpeakKey(rule)] : "Negative Exponent";
                    break;
            }

            return ruleName;
        }

        // Gets the rule name speak key
        public static string GetRuleNameSpeakKey(exponentRule rule)
        {
            // Sets the rule name speak key.
            string speakKey = "";

            // If the LOLManager wasn't instantiated, then neither was
            if (!LOLManager.IsLOLSDKInitialized())
                return "";

            // Checks the rule.
            switch (rule)
            {
                default:
                case exponentRule.none:
                    speakKey = "rle_none_nme";
                    break;

                case exponentRule.exponent:
                    speakKey = "rle_exponent_nme";
                    break;

                case exponentRule.product:
                    speakKey = "rle_product_nme";
                    break;

                case exponentRule.powerOfAPower:
                    speakKey = "rle_powerOfAPower_nme";
                    break;

                case exponentRule.powerOfAProduct:
                    speakKey = "rle_powerOfAProduct_nme";
                    break;

                case exponentRule.zero:
                    speakKey = "rle_zero_nme";
                    break;

                case exponentRule.negative:
                    speakKey = "rle_negative_nme";
                    break;
            }

            return speakKey;
        }

        // Gets the rule description.
        public static string GetRuleDescription(exponentRule rule)
        {
            // TODO: rewrite so that they make more sense.

            // Sets the rule description.
            string ruleDesc = "";

            // Translation.
            JSONNode defs = (LOLManager.IsLOLSDKInitialized()) ? SharedState.LanguageDefs : null;

            // Checks the rule.
            switch (rule)
            {
                default:
                case exponentRule.none:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "No exponent rule.";
                    break;

                case exponentRule.exponent:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "Multiplies a term (n) number of times.";
                    break;

                case exponentRule.product:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "Exponents with the same base are equal to said base to the sum of the exponents.";
                    break;

                case exponentRule.powerOfAPower:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "A base and exponent to another exponent is equal to the base to the product of said exponents.";
                    break;

                case exponentRule.powerOfAProduct:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "Two bases multiplied together with the same exponents is equal to the two bases multiplied together to the power of the exponent.";
                    break;

                case exponentRule.zero:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "A zero exponent always rules in a 1.";
                    break;

                case exponentRule.negative:
                    ruleDesc = (defs != null) ? defs[GetRuleDescriptionSpeakKey(rule)] :
                        "A negative exponent becomes 1/x.";
                    break;
            }

            return ruleDesc;
        }

        // Gets the rule description speak key.
        public static string GetRuleDescriptionSpeakKey(exponentRule rule)
        {
            // Sets the rule description speak key.
            string speakKey = "";

            // If the LOLManager wasn't instantiated, then neither was
            if (!LOLManager.IsLOLSDKInitialized())
                return "";

            // Checks the rule.
            switch (rule)
            {
                default:
                case exponentRule.none:
                    speakKey = "rle_none_dsc";
                    break;

                case exponentRule.exponent:
                    speakKey = "rle_exponent_dsc";
                    break;

                case exponentRule.product:
                    speakKey = "rle_product_dsc";
                    break;

                case exponentRule.powerOfAPower:
                    speakKey = "rle_powerOfAPower_dsc";
                    break;

                case exponentRule.powerOfAProduct:
                    speakKey = "rle_powerOfAProduct_dsc";
                    break;

                case exponentRule.zero:
                    speakKey = "rle_zero_dsc";
                    break;

                case exponentRule.negative:
                    speakKey = "rle_negative_dsc";
                    break;
            }

            return speakKey;
        }
    }
}