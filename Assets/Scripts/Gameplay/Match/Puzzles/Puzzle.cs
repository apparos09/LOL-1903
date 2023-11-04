using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EM
{
    // The exponent rules.
    /*
     * Exponent Rule (Basic Exponent Rule): a^n
     * Product Rule (Multiplication (Same Base)): (a^n)*(a^m) = a^(n+m)
     * Power of a Power (Exponent by Exponent): (a^n)^m = a^(n*m)
     * Power of a Product (Multplication (Different Base)): (a^n)*(b^n) = (a*b)^n
     * Zero Exponent (Zero): a^0 = 1
     * Negative Exponent (Negative): a^(-1) = 1/(a^n)
     */
    public enum exponentRule { none, exponent, product, powerPower, powerProduct, zero, negative }

    // Generates content for a puzzle.
    public class Puzzle : MonoBehaviour
    {
        // The puzzle types.
        public enum puzzleType { unknown, keypad, bubbles, sliding, pinball }

        // A value space.
        public struct ValueSpace
        {
            // A value and its index.
            public char value;
            public int index;
        }

        // A puzzle question set. These are combined with + or - signs...
        // Since those don't combine exponent terms with the end result.
        public struct PuzzleCalculation
        {
            // The rule the calculation follows. If it has multiple (or no) rules, set ot none.
            public exponentRule rule;

            // The question.
            public string question;

            // The answer.
            public string answer;
            
        }


        // The manager for the match.
        public MatchManager manager;

        // The puzzle type.
        public puzzleType puzzle;

        // The mechanic of the puzzle.
        public PuzzleMechanic puzzleMechanic;

        // The renderer for the puzzle space.
        public PuzzleRender puzzleRender;

        // The position of the puzzle mechanic.
        public GameObject puzzleMechanicPos;
        
        // The player this puzzle belongs to. If no player is set, then any player can interact with it.
        public PlayerMatch playerMatch;

        // The amount of time it took to give an answer.
        [Tooltip("The elapsed time for how long it took to correctly answer the question.")]
        public float answerTime = 0.0F;

        [Header("Exponents")]

        // NOTE: a rate of 0 or less means it will never appear.

        // The exponent rate (base exponent rate).
        public float exponentRate = 1.0F;

        // Rate for multiplicaton (same bases)/product rule exponents.
        public float productRate = 1.0F;

        // Rate for exponent by exponent/power of a power exponents.
        public float powerOfAPowerRate = 1.0F;

        // Rate for multplication (different bases)/power of a product exponents.
        public float powerOfAProductRate = 1.0F;

        // Rate for zero exponents.
        public float zeroRate = 1.0F;
        
        // Rate for negative exponents.
        public float negativeRate = 1.0F;

        [Header("Equation")]

        // The question equation
        [Tooltip("The base/solved equation.")]
        public string equation = "";

        // The current state of the equation (has missing values).
        [Tooltip("The equation question (the one with missing values).")]
        public string equationQuestion = "";

        // This symbol is used to represent spaces in the equation to be filled.
        public const string EQUATION_SPACE = "$";

        // The rules the current equation is using.
        public List<exponentRule> rulesUsed = new List<exponentRule>();

        // The lowest value can equation will use.
        public int equationLowestValue = 0;

        // The highest value an equation will use.
        public int equationHighestValue = 9;

        
        // TERMS //
        [Header("Equation/Terms")]

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

        
        // MISSING VALUES //
        [Header("Equations/Missing Values")]

        // The number of values minimun and maximum. If you don't want it randomized, set it to the same values.
        // These values must be greater than 0.
        // Minimum Number of Missing Values
        [Tooltip("The minimum number of missing values.")]
        public int missingValuesMin = 1;

        // Maximum Number of Missing Values
        [Tooltip("The maximum number of missing values.")]
        public int missingValuesMax = 1;

        // The values that must be found for the equation.
        // Once this queue is empty, the equation has been solved.
        // This is filled from back to front, hence why it's a stack.
        public Stack<ValueSpace> missingValues = new Stack<ValueSpace>();

        // The starting number of missing values.
        private int missingValuesCountStart = 0;

        // The minimum amount of random values available when asking for a random value.
        private const int RANDOM_VALUE_MIN = 5;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance.
            if (manager == null)
                manager = MatchManager.Instance;

            // Generates a equation to start off (not needed).
            // GenerateEquation();
        }

        // Gets the rule name.
        public static string GetRuleName(exponentRule rule)
        {
            // Sets the rule name.
            string ruleName = "";

            // Checks the rule.
            switch(rule)
            {
                default:
                case exponentRule.none:
                    ruleName = "None";
                    break;

                case exponentRule.exponent:
                    ruleName = "Exponent";
                    break;

                case exponentRule.product:
                    ruleName = "Product Rule";
                    break;

                case exponentRule.powerPower:
                    ruleName = "Power of a Power Rule";
                    break;

                case exponentRule.powerProduct:
                    ruleName = "Power of a Product Rule";
                    break;

                case exponentRule.zero:
                    ruleName = "Zero Rule";
                    break;

                case exponentRule.negative:
                    ruleName = "Negative Exponent Rule";
                    break;
            }

            return ruleName;
        }

        // Generates and results a calculation.
        /*
         * rule: the exponent rule being used.
         * lowestValue: the lowest value being used in the question.
         * highestValue: the highest value being used in the question (inclusive).
         * termsMin: the minimum amount of terms (term count is randomized).
         * termsMax: the maximum amount of terms (term count is randomized).
         */
        public static PuzzleCalculation GenerateCalculation(exponentRule rule, int lowestValue, int highestValue, int termsMin, int termsMax)
        {
            /*
             * expo (Base Exponent Rule): a^n
             * multSame (Multiplication (Same Base)): (a^n)*(a^m) = a^(n+m)
             * expoByExpo (Exponent by Exponent): (a^n)^m = a^(n*m)
             * multDiff (Multplication (Different Base)): (a^n)*(b^n) = (a*b)^n
             * zero (Zero): a^0 = 1
             * negative (Negative): a^(-1) = 1/(a^n)
             */

            // Make sure to place brackets around terms that go in exponents.

            // The resulting calculation.
            PuzzleCalculation resultCalc = new PuzzleCalculation();

            // The sub calculations to be combined into the result calcuation.
            List<PuzzleCalculation> subCalcs = new List<PuzzleCalculation>();

            // The term count. 
            int termCount = Random.Range(termsMin, termsMax);

            // Checks the rule being applied.
            switch(rule)
            {
                case exponentRule.exponent: // Exponent (Base)
                default:
                    
                    {
                        PuzzleCalculation calc = new PuzzleCalculation();

                        // Set the rule.
                        calc.rule = rule;

                        // The number that will have the exponent applied to it.
                        int num = Random.Range(lowestValue, highestValue + 1);

                        // Sets the number to 1 if it's set to 0 or less.
                        // TODO: maybe set it to 2 instead of 1?
                        if (num <= 0)
                            num = 1;

                        // Clear out the question.
                        calc.question = "";

                        // TODO: should you allow exponents to 1?
                        // If the terms total is equal to 1, push it up to 2?
                        int termsTotal = (termCount > 1) ? termCount : 2;

                        // Goes through each term generation.
                        for (int n = 1; n <= termsTotal; n++)
                        {
                            // Add the number.
                            calc.question += num.ToString();

                            // If this isn't the last term, add the multiplication symbol.
                            if (n < termsTotal)
                                calc.question += "*";
                        }

                        // Gets the answer.
                        calc.answer = num.ToString() + "^(" + termsTotal.ToString() + ")";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }

                    break;

                case exponentRule.product: // Multplication (Same Base)
                    // Goes through each term generation.
                    for (int n = 1; n <= termCount; n++)
                    {
                        PuzzleCalculation calc = new PuzzleCalculation();

                        // Set the rule.
                        calc.rule = rule;

                        // Expression: (a^n)*(a^m) = a^(n+m)
                        int num1 = Random.Range(lowestValue, highestValue + 1); // a (base)
                        int num2 = Random.Range(lowestValue, highestValue + 1); // n (exponent 1)
                        int num3 = Random.Range(lowestValue, highestValue + 1); // m (exponent 2)

                        // Empty the string.
                        calc.question = "";

                        // Number 1 (Base) and Exponent 1 - (a^n)
                        calc.question += "(" + num1.ToString() + "^(" + num2.ToString() + "))";

                        // Multplication Symbol
                        calc.question += "*";

                        // Number 1 (Base) and Exponent 2 (a^m)
                        calc.question += "(" + num1.ToString() + "^(" + num3.ToString() + "))";

                        // The Answer - a^(n+m)
                        calc.answer = num1.ToString() + "^(" + num2.ToString() + "+" + num3.ToString() + ")";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }

                    break;

                case exponentRule.powerPower: // Exponent by Exponent
                    // Goes through each term generation.
                    for (int n = 1; n <= termCount; n++)
                    {
                        PuzzleCalculation calc = new PuzzleCalculation();

                        // Set the rule.
                        calc.rule = rule;

                        // Expression: (a^n)^m = a^(n*m)
                        // Generates numbers 1-3.
                        int num1 = Random.Range(lowestValue, highestValue + 1); // a (base)
                        int num2 = Random.Range(lowestValue, highestValue + 1); // n (exponent 1)
                        int num3 = Random.Range(lowestValue, highestValue + 1); // m (exponent 2)

                        // Empty the string.
                        calc.question = "";

                        // Number 1 (Base) and Number 2 (Exponent 1/Inside)
                        calc.question += "(" + num1.ToString() + "^(" + num2.ToString() + "))";

                        // Number 3 (Exponent 2/Outside).
                        calc.question += "^(" + num3.ToString() + ")";

                        // The Answer - a^(n*m)
                        calc.answer = num1.ToString() + "^(" + num2.ToString() + "*" + num3.ToString() + ")";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }

                    break;

                case exponentRule.powerProduct: // Multiplication (Different Base)

                    // Goes through each term generation.
                    for (int n = 1; n <= termCount; n++)
                    {
                        PuzzleCalculation calc = new PuzzleCalculation();

                        // Set the rule.
                        calc.rule = rule;

                        // Expression: (a^n)*(b^n) = (a*b)^n
                        // Generates numbers 1-3.
                        int num1 = Random.Range(lowestValue, highestValue + 1); // a (base 1)
                        int num2 = Random.Range(lowestValue, highestValue + 1); // b (base 2)
                        int num3 = Random.Range(lowestValue, highestValue + 1); // n (exponent)

                        // Empty the string.
                        calc.question = "";

                        // NOTE: the numbers use brackets to seperate the terms with their exponents.
                        // This shouldn't cause any issues, though it isn't necessary.

                        // Number 1 and Exponent.
                        calc.question += "(" + num1.ToString() + "^(" + num3.ToString() + "))";

                        // Multplication Sign.
                        calc.question += "*";

                        // Number 2 and Exponent.
                        calc.question += "(" + num2.ToString() + "^(" + num3.ToString() + "))";

                        // The Answer - (a*b)^n
                        calc.answer = "(" + num1.ToString() + "*" + num2.ToString() + ")^(" + num3.ToString() + ")";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }

                    break;

                case exponentRule.zero: // Zero

                    // Goes through each term generation.
                    for (int n = 1; n <= termCount; n++)
                    {
                        // Expression: a^0 = 1
                        // Generates a question and answer - the answer is always 1.
                        PuzzleCalculation calc = new PuzzleCalculation();
                        // Set the rule.
                        calc.rule = rule;

                        calc.question = Random.Range(lowestValue, highestValue + 1).ToString() + "^(0)";
                        calc.answer = "1";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }

                    break;

                case exponentRule.negative: // Negative
                    
                    // Goes through each term generation.
                    for (int n = 1; n <= termCount; n++)
                    {
                        PuzzleCalculation calc = new PuzzleCalculation();

                        // Set the rule.
                        calc.rule = rule;

                        // Expression: a^(-1) = 1/(a^n)
                        // Generates number 1 and number 2.
                        int num1 = Random.Range(lowestValue, highestValue + 1); // a (base)
                        int num2 = Random.Range(lowestValue, highestValue + 1); // b (exponent)

                        // There's no such thing as negative 0, so if num2 becomes 0, it defaults to 1.
                        if (num2 == 0)
                            num2 = 1;

                        // Empty string to start.
                        calc.question = "";

                        // Number 1, left bracket, and negative. 
                        calc.question = num1.ToString() + "^(-";

                        // Number 2, right backet.
                        calc.question += num2.ToString() + ")";

                        // The answer.
                        calc.answer = "1/" + num1.ToString() + "^(" + num2.ToString() + ")";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }
                    break;
            }

            // Set the rule for the resulting calculation.
            resultCalc.rule = rule;

            // Puts all the calculations in the result calculation.
            for(int i = 0; i < subCalcs.Count; i++)
            {
                // Add the question and the answer.
                resultCalc.question += subCalcs[i].question;
                resultCalc.answer += subCalcs[i].answer;

                // If this isn't the last one, add a (+) or (-) sign at the end.
                if(i != subCalcs.Count - 1)
                {
                    // Determine the symbol.
                    int randInt = Random.Range(1, 3);
                    string symbol = randInt <= 1 ? "+" : "-";

                    // Add the symbol.
                    resultCalc.question += symbol;
                    resultCalc.answer += symbol;
                }

            }

            // Returns the result calculation.
            return resultCalc;
        }


        // Generates a calculation and puts it together into a string.
        public static string GenerateCalculationAsString(exponentRule rule, int lowestValue, int highestValue, int termsMin, int termsMax)
        {
            // Generates the calculation.
            PuzzleCalculation calc = GenerateCalculation(rule, lowestValue, highestValue, termsMin, termsMax);
            string result = calc.question + "=" + calc.answer;
            
            // Returns the result.
            return result;
        }
        
        // TODO: remove this.
        // Combines the calculations into 1 string, using the plusRate and minusRate to determine if they're...
        // Connected by plus signs or minus signs (equal chance of both)
        private string CombineCalculations(List<PuzzleCalculation> calcs)
        {
            // The questions combined.
            string question = "";

            // The answers combined.
            string answer = "";

            // The end result.
            string result = "";

            for(int i = 0; i < calcs.Count; i++)
            {
                // Generate the question.
                question += calcs[i].question;
                answer += calcs[i].answer;

                // If not on the last index.
                if(i + 1 < calcs.Count)
                {
                    // Generate the symbol.
                    string symbol = (Random.Range(1, 3) == 1) ? "+" : "-";

                    // Add the symbol.
                    question += symbol;
                    answer += symbol;
                }
                
            }

            // Combine all.
            result = question + "=" + answer;

            return result;
        }


        // Generates an equation to be filled.
        public void GenerateEquation()
        {
            // TODO: do equation calculation here.
            // Don't have spaces.
            
            // TODO: implement rule combination and value replacement rules.

            // TODO: you'll need to change this so that missing values can be generated properly.
            // equation = "3x^2*3x^(21)=3x^4"; // Test
            equation = GenerateCalculationAsString(exponentRule.exponent, 1, 9, 1, 5);

            // GENERATE THE EQUATION.

            // The number of terms for the equation.
            int termCount = 0;

            // If the terms are zero or less, set the term count to 1.
            if(equationTermsMin <= 0 || equationTermsMax <= 0)
            {
                termCount = 1;
            }
            else
            {
                termCount = Random.Range(equationTermsMin, equationTermsMax + 1);
            }


            // The puzzle calculations
            List<PuzzleCalculation> calcs = new List<PuzzleCalculation>();

            // The exponent rules.
            exponentRule[] expoRules = new exponentRule[6]
            {
                exponentRule.exponent, exponentRule.product,
                exponentRule.powerPower, exponentRule.powerProduct,
                exponentRule.zero, exponentRule.negative
            };

            // The exponent chance rates.
            float[] expoRates = new float[6];
            

            // The rate sum;
            float rateSum = 0.0F;

            // Generates all the exponent rules.
            expoRates[0] = exponentRate;
            expoRates[1] = expoRates[0] + productRate;
            expoRates[2] = expoRates[1] + powerOfAPowerRate;
            expoRates[3] = expoRates[2] + powerOfAProductRate;
            expoRates[4] = expoRates[3] + zeroRate;
            expoRates[5] = expoRates[4] + negativeRate;

            // Sums up the rates.
            foreach(float f in expoRates)
            {
                rateSum += f;
            }


            // Adds each term.
            for(int n = 1; n <= termCount; n++)
            {
                // The rule being used.
                exponentRule rule = exponentRule.none;

                // Random float.
                float randFloat = 0;

                // Generates a random float.
                randFloat = Random.Range(0.0F, rateSum);

                // TODO: when you set it to only use zero rule, it ended up giving you base exponent rule too.
                // Figure out how to fix that.

                // The index of the chosen exponent rule.
                int index = 0;

                // Goes though all the exponent rates to see which exponent should be chosen.
                for (int i = 0; i < expoRates.Length; i++)
                {
                    // The index has been found, so break it.
                    if (randFloat <= expoRates[i] && expoRates[i] > 0)
                    {
                        index = i;
                        break;
                    }
                }

                // Grabs the rule to be used.
                rule = expoRules[index];

                // Generates a calculation using the rule.
                PuzzleCalculation calc;
                
                // Checks the rule for how to generate the calculation.
                switch(rule)
                {
                    case exponentRule.exponent: // Base Exponent
                        calc = GenerateCalculation(rule,
                            equationLowestValue, equationHighestValue, baseExponentTermsMin, baseExponentTermsMax);

                        break;

                    default: // Regular
                        calc = GenerateCalculation(rule,
                            equationLowestValue, equationHighestValue, 1, 1);
                        
                        break;
                }

                // Adds the calculation to the list.
                calcs.Add(calc);
            }

            // NEW (WIP)

            // Step 1. Erase values from back to start, going from the end of the calcs list to the beginning.
            // Step 2. Seperate the values between left side and right side of the equals sign.
            // Step 3. Put the missing values into queues at first. When finished, put them all into the stack.

            // The number of values to replace.
            int replaceCount;

            // The usable indexes.
            List<int> usableIndexes;

            // The selected indexes for replacement.
            List<int> replaceIndexes;

            // Makes a copy of the calcs list for combining the calculations later.
            List<PuzzleCalculation> blankCalcs = new List<PuzzleCalculation>(calcs);

            // TODO: convert this into a queue so that you can combine them all into one stack.
            // The queue for the values left and right of the equals sign.
            Queue<ValueSpace> leftQueue = new Queue<ValueSpace>();
            Queue<ValueSpace> rightQueue = new Queue<ValueSpace>();

            
            // Goes through each calculation to blank out the values.
            for(int i = calcs.Count - 1; i >= 0; i--)
            {
                // Set to 0.
                replaceCount = 0;

                // Determines if a '1' should be used, or a random blank out amount.
                replaceCount = (missingValuesMin <= 0 || missingValuesMax <= 0) ? 
                    1 : Random.Range(missingValuesMin, missingValuesMax + 1);


                // FINDING THE INDEXES
                // The list of indexes in the equation to choose from.
                usableIndexes = new List<int>();

                // The selected indexes.
                replaceIndexes = new List<int>();

                // BLACKLISTED VALUES: ^, (, ), =

                // TODO: for now, you're only going to erase values either to the left or the right of the equals sign...
                // As that guarantees the question can be answered. You should mix it up with values to the left and right of the equals sign...
                // Later on (make changes later).

                // Determines what side of the equals sign to use.
                // 1 = Left, 2 = Right
                bool leftEquals = (Random.Range(1, 3) == 1) ? true : false;

                // Grabs the expression.
                string expression = (leftEquals) ? calcs[i].question : calcs[i].answer;

                // Use this later (or not?)
                /*
                switch(calc.rule)
                {
                    case exponentRule.none:
                    default:
                        // Nothing
                        break;

                    case exponentRule.expo:
                        break;

                    case exponentRule.multSame:
                        break;

                    case exponentRule.expoByExpo:
                        break;

                    case exponentRule.multDiff:
                        break;

                    case exponentRule.zero:
                        break;

                    case exponentRule.negative:

                        break;
                }
                */


                // Blanks out values in the expression - adds each index to the list.
                for (int j = 0; j < expression.Length; j++)
                {
                    // If the index is not an exponent symbol, a bracket, or an equals sign.
                    if (expression[j] != '^' && expression[j] != '(' && expression[j] != ')' && expression[j] != '=')
                    {
                        // Handles custom specifications for usable indexes in current rules.
                        switch(calcs[i].rule)
                        {
                            case exponentRule.zero: // Zero
                                
                                // Checks if the exponent symbol exists in the expression(which it always should).
                                if(expression.Contains("^"))
                                {
                                    // Makes sure (j) is greater than, or equal to the index of the exponent.
                                    // This way the question can always be answered (a^0 = 1).
                                    if (j >= expression.IndexOf("^"))
                                    {
                                        usableIndexes.Add(j);
                                    }
                                }
                                else // No exponent symbol.
                                {
                                    usableIndexes.Add(j);
                                }

                                break;
                                
                            default: // Default
                                usableIndexes.Add(j);
                                break;
                        }
                    }
                }


                // If the count is not set to 0.
                if(usableIndexes.Count != 0)
                {
                    // Finds what values will be replaced.
                    for (int j = 0; j < replaceCount; j++)
                    {
                        // Generate a random index.
                        int randIndex = Random.Range(0, usableIndexes.Count);

                        // Add the index to the replacment list, and remove it from the list of available indexes.
                        replaceIndexes.Add(usableIndexes[randIndex]);
                        usableIndexes.RemoveAt(randIndex);
                    }
                }


                // Make the replacement indexes list go from smallest to largest.
                replaceIndexes.Sort();


                // REPLACING VALUES //
                // Puts the replace values in the queue.

                // Replaces each index in the equation question, putting it into the blankCalcs list.
                for (int j = replaceIndexes.Count - 1; j >= 0; j--)
                {
                    // Remove the value at the index.
                    string temp = expression;

                    // Pushes the value that's going to be replaced into the missing values stack.
                    // Generates a value space object, giving it the value and the index it belongs to.
                    ValueSpace vs = new ValueSpace();
                    vs.value = expression[replaceIndexes[j]];
                    vs.index = replaceIndexes[j];

                    // Puts it into the appropriate queue based on...
                    // If it's to the left or the right of the equals sign.
                    if (leftEquals)
                        leftQueue.Enqueue(vs);
                    else
                        rightQueue.Enqueue(vs);

                    // Removes the value from temp.
                    temp = temp.Remove(replaceIndexes[j], 1);

                    // Insert the placeholder for being filled in.
                    temp = temp.Insert(replaceIndexes[j], EQUATION_SPACE);

                    // Update the question.
                    expression = temp;
                }

                // Set this as the expression, and put it back in the blank calcs list.
                if (leftEquals) // Update left of equals.
                {
                    PuzzleCalculation calc = blankCalcs[i];
                    calc.question = expression;
                    blankCalcs[i] = calc;
                }
                else // Update right of equals.
                {
                    PuzzleCalculation calc = blankCalcs[i];
                    calc.answer = expression;
                    blankCalcs[i] = calc;
                }
                    
            }


            // Combines the expressions.
            {
                // The questions combined (left side of equals sign).
                string questionSolved = "";
                string questionBlank = "";

                // The answers combined (right side of equals sign).
                string answerSolved = "";
                string answerBlank = "";

                // The end results.
                string resultSolved = "";
                string resultBlank = "";

                // Clears out the number of rules used.
                rulesUsed.Clear();

                // Goes through all the calculations.
                for (int i = 0; i < calcs.Count; i++)
                {
                    // Puts in the questions.
                    questionSolved += calcs[i].question;
                    questionBlank += blankCalcs[i].question;

                    // Puts in the answers.
                    answerSolved += calcs[i].answer;
                    answerBlank += blankCalcs[i].answer;


                    // Saves the rule used if it is not already in the list.
                    if (!rulesUsed.Contains(calcs[i].rule))
                        rulesUsed.Add(calcs[i].rule);

                    // If not on the last index.
                    if (i + 1 < calcs.Count)
                    {
                        // Generate the symbol.
                        string symbol = (Random.Range(1, 3) == 1) ? "+" : "-";

                        // Add the symbol to the questions.
                        questionSolved += symbol;
                        questionBlank += symbol;

                        // Add the symbol to the answers.
                        answerSolved += symbol;
                        answerBlank += symbol;
                    }

                }

                // Combine both sides of the equal signs.
                resultSolved = questionSolved + "=" + answerSolved;
                resultBlank = questionBlank + "=" + answerBlank;

                // Fill in the missing values.
                missingValues.Clear();

                // The index of the current missing character.
                int currMissIndex = resultBlank.Length;

                // The front of the queue is the last missing value for each side (it goes from end to beginning).
                // By combining the queues into a stack this way, the values are in the proper order.
                // Relatedly, when combined into one equation, the saved indexes are made wrong...
                // So they need to be corrected.

                // Right of equals sign (answer)
                while (rightQueue.Count != 0)
                {
                    // Index of the proper equation space, going from the end to the beginning.
                    currMissIndex = resultBlank.LastIndexOf(EQUATION_SPACE, currMissIndex - 1);
                    
                    // Gets the value space, and correct's the index.
                    ValueSpace vs = rightQueue.Dequeue();
                    vs.index = currMissIndex;

                    // Puts it in the list.
                    missingValues.Push(vs);
                }

                // Left of equals sign (question).
                while (leftQueue.Count != 0)
                {
                    // Index of the proper equation space, going from the end to the beginning.
                    currMissIndex = resultBlank.LastIndexOf(EQUATION_SPACE, currMissIndex - 1);

                    // Gets the value space, and correct's the index.
                    ValueSpace vs = leftQueue.Dequeue();
                    vs.index = currMissIndex;

                    // Puts it in the list.
                    missingValues.Push(vs);
                }

                // Set as the equation and equation question.
                equation = resultSolved;
                equationQuestion = resultBlank;

                // Save the count of missing values.
                missingValuesCountStart = missingValues.Count;
            }

        }

        // Returns a random value.
        // if 'useLimit' is true, the random value is limited to a certain set of values.
        public char GetRandomPuzzleValue(bool limitValues = true)
        {
            // TODO: have this limit the value based on what's needed for the actual question.
            // The value to be returned.
            char value;

            // The list of available chars.
            // Numbers 0-9, Plus, Minus, Multiply, and Divide
            List<char> valueList = new List<char>
            {
                '0', '1', '2', '3', '4',
                '5', '6', '7', '8', '9',
                '+', '-', '*', '/'
            };

            // A random index variable.
            int randIndex = -1;

            // Checks if the limit should be used, and there are missing values.
            if(limitValues && missingValues.Count > 0)
            {
                // Creates a list copy of the missing values.
                List<ValueSpace> valueSpaces = new List<ValueSpace>(missingValues);

                // TODO: should I remove duplicates? Maybe not.

                // While the random value count is below the minimum.
                while(valueSpaces.Count < RANDOM_VALUE_MIN)
                {
                    // Generate a random value.
                    randIndex = Random.Range(0, valueList.Count);

                    // Creates a new value space object.
                    ValueSpace vs = new ValueSpace();
                    vs.value = valueList[randIndex];
                    vs.index = -1;

                    // Puts the random value in the value spaces list.
                    valueSpaces.Add(vs);
                }

                // Generates a random index of the values.
                randIndex = Random.Range(0, valueSpaces.Count);
                value = valueSpaces[randIndex].value;
            }
            else // Equally-weighted value randomizer.
            {
                // Generate a random value.
                randIndex = Random.Range(0, valueList.Count);

                // Sets the value from the random index.
                value = valueList[randIndex];
            }
            
            // Returns the value.
            return value;
        }

        // Tries to select a puzzle element. Override if a puzzle has custom elements.
        public virtual void SelectElement(PlayerMatch player, GameObject hitObject)
        {
            // If the player has been set.
            if(playerMatch != null)
            {
                // If the player that selected the element isn't the right player, do nothing.
                if (playerMatch != player)
                    return;
            }

            // Puzzle value object.
            PuzzleValue value;

            // Other
            PinballGate gate;

            // If a puzzle value was grabbed from the selected element.
            if(hitObject.TryGetComponent(out value))
            {
                SelectValue(player, value);
            }

            // Checks if the selected object is a pinball gate.
            if(hitObject.TryGetComponent(out gate))
            {
                gate.OnInteract();
            }
        }

        // TODO: add select value function.
        public void SelectValue(PlayerMatch player, PuzzleValue value)
        {
            // If the player has been set.
            if (playerMatch != null)
            {
                // If the player that selected the element isn't the right player, do nothing.
                if (playerMatch != player)
                    return;
            }


            // No mising values.
            if (missingValues.Count == 0)
                return;


            // Checks if it was the correct value.
            bool rightValue = value.value == missingValues.Peek().value;

            // TODO: remove log calls.
            // Conditional for right and wrong value.
            if (rightValue)
            {
                Debug.Log("Right!");

                // Grabs the value space.
                ValueSpace vs = missingValues.Pop();

                // Removes the placeholder and replaces it with its proper value.
                equationQuestion = equationQuestion.Remove(vs.index, 1);
                equationQuestion = equationQuestion.Insert(vs.index, vs.value.ToString());

                // Updates the display based on the player.
                if (player == manager.p1)
                    manager.matchUI.UpdatePlayer1EquationDisplay();
                else if (player == manager.p2)
                    manager.matchUI.UpdatePlayer2EquationDisplay();

            }
            else
            {
                Debug.Log("Wrong!");

                // Checks if the gameplay object exists.
                if(GameplayInfo.Instantiated)
                {
                    // Gets the instance.
                    GameplayInfo gameInfo = GameplayInfo.Instance;

                    // If player 1 got it wrong, increase the number of wrong answers.
                    if (player == manager.p1)
                    {
                        gameInfo.wrongAnswers++;
                    }
                }
            }

            // TODO: maybe move this inside the "right" bracket?
            if (missingValues.Count == 0)
            {
                // Call the manager to say that the equation has been completed.
                manager.OnEquationComplete(
                    this, player, equation, rulesUsed, missingValuesCountStart, answerTime);

                // Sets the answer time to 0.
                answerTime = 0.0F;
            }

            // Call the OnHit function for the value.
            value.OnHit(rightValue);

        }

        // TODO: not needed?
        //// Call this function to use the power on the puzzle.
        //public void ApplyPower(Power power)
        //{
        //    // Applies the effect of the provided power.
        //    // TODO: implement.
        //}

        // Skips the current equation.
        public void SkipEquation()
        {
            answerTime = 0.0f;
            GenerateEquation();
            manager.OnEquationSkipped(this);
        }

        // Gets the question with proper formattng.
        public string GetEquationQuestionFormatted()
        {

            // TODO: you need to make sure you format it properly for exponent notation (superscript).
            // TODO: implement colour?

            // The result.
            string result = equationQuestion;

            // The current index.
            int index = result.Length - 1;

            // Go from the end of the string to the begining, while the index is valid.
            while(index >= 0 && index < result.Length && result.Contains("^"))
            {
                // Finds the last index of the "^" symbol.
                index = result.LastIndexOf("^", index);

                // Index found.
                if (index != -1 && index != result.Length - 1)
                {
                    // Checks if the next character is a bracket or not.
                    if (result[index + 1] == '(')
                    {
                        // NOTE: this assumes the brackets are setup properly.
                        // Since the questions are generated by the code, there should never be a broken formatting case.
                        // As such, it shouldn't need to be addressed.

                        // Gets the index of the left bracket and the right bracket.
                        int leftIndex = index + 1;
                        int rightIndex = result.IndexOf(')', leftIndex + 1);

                        // Removes the right bracket and replaces it with the 'end' of the superscript code.
                        result = result.Remove(rightIndex, 1);
                        result = result.Insert(rightIndex, "</sup>");

                        // Removes the left bracket and replaces it with the 'beginning' of the supescript code.
                        result = result.Remove(leftIndex, 1);
                        result = result.Insert(leftIndex, "<sup>");

                        // Remove the "^" (exponent) symbol.
                        result = result.Remove(index, 1);
                    }
                    else // No bracket, so just apply superscript to rightward character.
                    {
                        result = result.Insert(index + 2, "</sup>"); // End
                        result = result.Remove(index, 1); // Remove "^" (exponent sign
                        result = result.Insert(index, "<sup>"); // Start

                    }
                }
            }

            // TODO: add in value missing sprite.

            // // Replace the multiplication symbols with an x.
            // result = result.Replace("*", "X");

            // Replaces the equation space with the on-screen space.
            result = result.Replace(EQUATION_SPACE, "[   ]");

            // Returns the result.
            return result;
        }
        
        // Calculates the amount of points for the answer pseed.
        public int GetPointsForAnswerSpeed(int minPoints, int maxPoints)
        {
            // TODO: this should scale with the puzzle type.
            // The par time.
            float parTime = 0.0F;
            
            // A multiple for the par time.
            float mult = 1.0F;

            // Checks the puzzle type to help see how much extra time there is per value.
            switch(puzzle)
            {
                case puzzleType.keypad:
                    mult = 1.0F;
                    break;

                case puzzleType.sliding:
                    mult = 1.25F;
                    break;

                case puzzleType.bubbles:
                    mult = 1.4F;
                    break;

                case puzzleType.pinball:
                    mult = 1.5F;
                    break;
            }

            // Calculates the par time using the missing values, the base time, and the mechanic modifier.
            // By default, each value fill is 5 seconds at the least. The par time is at least 1 second.
            parTime = 1.0F + missingValuesCountStart * 4.0F * mult;

            // Calculates how close the answer time is to the partime.
            float percent = 0.0F;

            // If the answer time is overtime, or less than 0, set the percent to 0.
            if(answerTime > parTime || answerTime < 0.0F)
            {
                percent = 0.0F;
            }
            else // Calculates the percentage of time left for points calculation.
            {
                percent = 1.0F - answerTime / parTime;
            }

            // Clamps the percentage.
            percent = Mathf.Clamp01(percent);

            // Calculates points as float.
            float pointsFlt = Mathf.Lerp(minPoints, maxPoints, percent);

            // Calculates points as int (round to whole number).
            int pointsInt = Mathf.CeilToInt(pointsFlt);

            // Return the result.
            return pointsInt;
        }

        // Update is called once per frame
        void Update()
        {
            // If the match isn't paused.
            if(!manager.MatchPaused)
            {
                // Add to answer time.
                answerTime += Time.deltaTime;
            }
        }
    }
}