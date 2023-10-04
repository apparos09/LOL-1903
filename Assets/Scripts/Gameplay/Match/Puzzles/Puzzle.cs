using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EM
{
    // The puzzle types.
    public enum puzzle { keypad, slider, bubble, pinball }

    // The exponent rules.
    /*
     * expo (Base Exponent Rule): a^n
     * multSame (Multiplication (Same Base)): (a^n)*(a^m) = a^(n+m)
     * expoByExpo (Exponent by Exponent): (a^n)^m = a^(n*m)
     * multDiff (Multplication (Different Base)): (a^n)*(b^n) = (a*b)^n
     * zero (Zero): a^0 = 1
     * negative (Negative): a^(-1) = 1/(a^n)
     */
    public enum exponentRule { none, expo, multSame, expoByExpo, multDiff, zero, negative }

    // Generates content for a puzzle.
    public class Puzzle : MonoBehaviour
    {
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

        // The renderer for the puzzle space.
        public PuzzleRender puzzleRender;

        // The player this puzzle belongs to.
        public PlayerMatch player;

        // The puzzle type.
        public puzzle puzzleType;

        [Header("Exponents")]

        // NOTE: a rate of 0 or less means it will never appear.

        // The exponent rates.
        public float expoRate = 1.0F;

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

        [Header("Equation")]

        // The question equation
        [Tooltip("The base/solved equation.")]
        public string equation = "";

        // The current state of the equation (has missing values).
        [Tooltip("The equation question (the one with missing values).")]
        public string equationQuestion = "";

        // This symbol is used to represent spaces in the equation to be filled.
        public const string EQUATION_SPACE = "$";

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
        public int missingValueMin = 1;

        // Maximum Number of Missing Values
        [Tooltip("The maximum number of missing values.")]
        public int missingValueMax = 1;

        // The values that must be found for the equation.
        // Once this queue is empty, the equation has been solved.
        // This is filled from back to front, hence why it's a stack.
        public Stack<ValueSpace> missingValues = new Stack<ValueSpace>();

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance.
            if (manager == null)
                manager = MatchManager.Instance;

            // Generates a equation to start off.
            GenerateEquation();

            // Gets the question, formatted.
            string formatted = GetEquationQuestionFormatted();

            // TODO: move this in the final game. This is only here for testing purposes.
            manager.matchUI.p1EquationText.text = formatted;
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
                case exponentRule.expo: // Exponent (Base)
                default:
                    
                    {
                        PuzzleCalculation calc = new PuzzleCalculation();

                        // Set the rule.
                        calc.rule = rule;

                        // The number that will have the exponent applied to it.
                        int num = Random.Range(lowestValue, highestValue + 1);

                        // Clear out the question.
                        calc.question = "";

                        // Goes through each term generation.
                        for (int n = 1; n <= termCount; n++)
                        {
                            // Add the number.
                            calc.question += num.ToString();

                            // If this isn't the last term, add the multiplication symbol.
                            if (n < termCount)
                                calc.question += "*";
                        }

                        // Gets the answer.
                        calc.answer = num.ToString() + "^(" + termCount.ToString() + ")";

                        // Adds to the sub calculations.
                        subCalcs.Add(calc);
                    }

                    break;

                case exponentRule.multSame: // Multplication (Same Base)
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

                case exponentRule.expoByExpo: // Exponent by Exponent
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

                case exponentRule.multDiff: // Multiplication (Different Base)

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
        
        // Combines the calculations into 1 string, using the plusRate and minusRate to determine if they're...
        // Connected by plus signs or minus signs.
        private string CombineCalculations(List<PuzzleCalculation> calcs)
        {
            // TODO: implement controls for plus rate and minus rate.

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
            equation = GenerateCalculationAsString(exponentRule.expo, 1, 9, 1, 5);

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
                exponentRule.expo, exponentRule.multSame,
                exponentRule.expoByExpo, exponentRule.multDiff,
                exponentRule.zero, exponentRule.negative
            };

            // The exponent chance rates.
            float[] expoRates = new float[6];
            

            // The rate sum;
            float rateSum = 0.0F;

            // Generates all the exponent rules.
            expoRates[0] = expoRate;
            expoRates[1] = expoRates[0] + multSameRate;
            expoRates[2] = expoRates[1] + expoByExpoRate;
            expoRates[3] = expoRates[2] + multDiffRate;
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
                    case exponentRule.expo: // Base Exponent
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

            // The number of values to replace.
            //int replaceCount;

            // The usable indexes.
            //List<int> usableIndexes;

            // The selected indexes for replacement.
            //List<int> replaceIndexes;

            //// TODO: convert this into a queue so that you can combine them all into one stack.
            //// Calc mussing values.
            //List<Stack<ValueSpace>> calcMissings = new List<Stack<ValueSpace>>();

            //// Blanks out values in each calculation.
            //foreach (PuzzleCalculation calc in calcs)
            //{
            //    // Set to 0.
            //    replaceCount = 0;

            //    // Determines if a '1' should be used, or a random blank out amount.
            //    replaceCount = (missingValueMin <= 0 || missingValueMax <= 0) ? 
            //        1 : Random.Range(missingValueMin, missingValueMax + 1);


            //    // FINDING THE INDEXES
            //    // The list of indexes in the equation to choose from.
            //    usableIndexes = new List<int>();

            //    // The selected indexes.
            //    replaceIndexes = new List<int>();

            //    // BLACKLISTED VALUES: ^, (, ), =

            //    // TODO: for now, you're only going to erase values either to the left or the right of the equals sign...
            //    // As that guarantees the question can be answered. You should mix it up with values to the left and right of the equals sign...
            //    // Later on (make changes later).

            //    // Determines what side of the equals sign to use.
            //    // 1 = Left, 2 = Right
            //    int equalsSide = Random.Range(1, 3);

            //    // Grab the expression.
            //    string expression = (equalsSide == 1) ? calc.question : calc.answer;

            //    // Use this later.
            //    /*
            //    switch(calc.rule)
            //    {
            //        case exponentRule.none:
            //        default:
            //            // Nothing
            //            break;

            //        case exponentRule.expo:
            //            break;

            //        case exponentRule.multSame:
            //            break;

            //        case exponentRule.expoByExpo:
            //            break;

            //        case exponentRule.multDiff:
            //            break;

            //        case exponentRule.zero:
            //            break;

            //        case exponentRule.negative:

            //            break;
            //    }
            //    */


            //    // Adds each index to the list.
            //    for (int i = 0; i < expression.Length; i++)
            //    {
            //        // If the index is not an exponent symbol, a bracket, or an equals sign.
            //        if (expression[i] != '^' && expression[i] != '(' && expression[i] != ')' && expression[i] != '=')
            //            usableIndexes.Add(i);
            //    }


            //    // If the count is not set to 0.
            //    if(usableIndexes.Count != 0)
            //    {
            //        // Finds what values will be replaced.
            //        for (int i = 0; i < replaceCount; i++)
            //        {
            //            // Generate a random index.
            //            int randIndex = Random.Range(0, usableIndexes.Count);

            //            // Add the index to the replacment list, and remove it from the list of available indexes.
            //            replaceIndexes.Add(usableIndexes[randIndex]);
            //            usableIndexes.RemoveAt(randIndex);
            //        }
            //    }


            //    // Goes from smallest to largest.
            //    replaceIndexes.Sort();


            //    // REPLACING VALUES //
            //    // Create a missing stack.
            //    Stack<ValueSpace> missingStack = new Stack<ValueSpace>();

            //    // Replaces each index in the equation question.
            //    for (int i = replaceIndexes.Count - 1; i >= 0; i--)
            //    {
            //        // Remove the value at the index.
            //        string temp = expression;

            //        // Pushes the value that's going to be replaced into the missing values stack.
            //        // Generates a value space object, giving it the value and the index it belongs to.
            //        ValueSpace vs = new ValueSpace();
            //        vs.value = expression[replaceIndexes[i]];
            //        vs.index = replaceIndexes[i];

            //        // Puts it on the stack.
            //        missingStack.Push(vs);

            //        // Removes the value.
            //        temp = temp.Remove(replaceIndexes[i], 1);

            //        // Insert the placeholder for being filled in.
            //        temp = temp.Insert(replaceIndexes[i], EQUATION_SPACE);

            //        // Update the question.
            //        expression = temp;
            //    }

            //    // Add the stack to the missing calcula
            //    calcMissings.Add(missingStack);
            //}


            // TODO: this REALLY needs to be simplifed.
            // Right now, you'd have to go from beginning to end, inverting the value order for each individaul stack...
            // Then combining them into one stack in the right order. This is because the question goes from left to right...
            // I'm going to add back in the old version for now for simplicity's sake, but this needs to be addressed.


            // OLD

            // Combines the calculations.
            equation = CombineCalculations(calcs);

            // Set the equation question.
            equationQuestion = equation;



            // TODO: implement rules on how missing values are deteremined.
            // The number of values to replace.
            int replaceCount = 0;

            // If both are above 0, do it at random. If both values are 0 or less, set it to 1.
            if (missingValueMin > 0 && missingValueMax > 0)
            {
                // If the values are the same, set it as said value. If the values are different, set it randomly.
                if (missingValueMin == missingValueMax)
                {
                    // Set replacement count.
                    replaceCount = missingValueMin;
                }
                else
                {
                    // Random replacement count.
                    replaceCount = Random.Range(missingValueMin, missingValueMax + 1);
                }
            }
            else
            {
                replaceCount = 1;
            }


            // FINDING THE INDEXES
            // The list of indexes in the equation to choose from.
            List<int> usableIndexes = new List<int>();

            // The selected indexes.
            List<int> replaceIndexes = new List<int>();

            // Adds each index to the list.
            for (int i = 0; i < equation.Length; i++)
            {
                // If the index is not an exponent symbol, a bracket, or an equals sign.
                if (equation[i] != '^' && equation[i] != '(' && equation[i] != ')' && equation[i] != '=')
                    usableIndexes.Add(i);
            }

            // Finds what values will be replaced.
            for (int i = 0; i < replaceCount && usableIndexes.Count != 0; i++)
            {
                // Generate a random index.
                int randIndex = Random.Range(0, usableIndexes.Count);

                // Add the index to the replacmeent list, and remove it from the list of available indexes.
                replaceIndexes.Add(usableIndexes[randIndex]);
                usableIndexes.RemoveAt(randIndex);
            }

            // Goes from smallest to largest.
            replaceIndexes.Sort();


            // REPLACING VALUES //
            // Clears out the missing values.
            missingValues.Clear();

            // Replaces each index in the equation question.
            for (int i = replaceIndexes.Count - 1; i >= 0; i--)
            {
                // Remove the value at the index.
                string temp = equationQuestion;

                // Pushes the value that's going to be replaced into the missing values stack.
                // Generates a value space object, giving it the value and the index it belongs to.
                ValueSpace vs = new ValueSpace();
                vs.value = equationQuestion[replaceIndexes[i]];
                vs.index = replaceIndexes[i];

                // Puts it on the stack.
                missingValues.Push(vs);

                // Removes the value.
                temp = temp.Remove(replaceIndexes[i], 1);

                // Insert the placeholder for being filled in.
                temp = temp.Insert(replaceIndexes[i], EQUATION_SPACE);

                // Update the question.
                equationQuestion = temp;
            }
        }

        // Tries to select a puzzle element. Override if a puzzle has custom elements.
        public virtual void SelectElement(GameObject hitObject)
        {
            PuzzleValue value;

            // If a puzzle value was grabbed from the selected element.
            if(hitObject.TryGetComponent(out value))
            {
                SelectValue(value);
            }
        }

        // TODO: add select value function.
        public void SelectValue(PuzzleValue value)
        {
            // No mising values.
            if (missingValues.Count == 0)
                return;


            // Checks if the selected value is correct.
            if(value.value == missingValues.Peek().value)
            {
                Debug.Log("Right!");

                // Grabs the value space.
                ValueSpace vs = missingValues.Pop();

                // Removes the placeholder and replaces it with its proper value.
                equationQuestion = equationQuestion.Remove(vs.index, 1);
                equationQuestion = equationQuestion.Insert(vs.index, vs.value.ToString());
            }
            else
            {
                Debug.Log("Wrong!");
            }
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

            // Go from the end of the string to the beggining, while the index is vali.
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

            // Replaces the equation space with the on-screen space.
            result = result.Replace(EQUATION_SPACE, "[   ]");

            // Returns the result.
            return result;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}