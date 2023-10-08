using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The information used to create the match when coming from the world scene.
    public class MatchInfo : MonoBehaviour
    {
        // The current time in teh game.
        public float gameTime = 0.0F;

        // The puzzle type.
        public puzzle puzzleType = puzzle.keypad;

        [Header("Exponents")]

        // The exponent rates.
        public float baseExpoRate = 1.0F;

        // Rate for multiplicaton (same bases) exponents.
        public float multSameRate = 1.0F;

        // Rate for exponent by exponent exponents.
        public float expoByExpoRate = 1.0F;

        // Rate for multplication (different bases) exponents.
        public float multDiffRate = 1.0F;

        // Rate for zero exponents.
        public float zeroRate = 1.0F;

        // Rate for negative exponents.
        public float negativeRate = 1.0F;

        [Header("Match/Puzzle Settings")]
        // The point goal for the game.
        public int pointGoal = 999;

        // If 'true', the point goal is used.
        public bool usePointGoal = true;

        // The lowest value can equation will use.
        public int equationLowestValue = 0;

        // The highest value an equation will use.
        public int equationHighestValue = 9;

        // Minimum equation term.
        [Tooltip("The minimum number of equation terms.")]
        public int equationTermsMin = 1;

        // Maximum equation term.
        [Tooltip("The maximum number of equation terms.")]
        public int equationTermsMax = 1;

        // The minimum number of terms for the base exponent rule.
        [Tooltip("The minimum number of terms for the base exponent rule (combined rules only).")]
        public int baseExponentTermsMin = 1;

        // The maximum number of terms for the base exponent rule.
        [Tooltip("The maximum number of terms for the base exponent rule (combined rules only).")]
        public int baseExponentTermsMax = 3;

        // The minimum number of missing values.
        [Tooltip("The minimum number of missing values.")]
        public int missingValuesMin = 1;

        // The maximum number of missing values.
        [Tooltip("The maximum number of missing values.")]
        public int missingValuesMax = 1;

        [Header("Challenger")]
        // The difficulty of the challenger.
        public int challengerDifficulty = 0;
    }
}