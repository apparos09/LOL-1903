using System.Collections;
using System.Collections.Generic;
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
    public enum exponentRule { expo, multSame, expoByExpo, multDiff, zero, negative }

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
            public string question;
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

        // The types of exponents being coveered.
        public List<exponentRule> expoRules = new List<exponentRule>();

        [Header("Equation")]

        // The question equation
        public string equation = "";

        // The current state of the equation (has missing values).
        public string equationQuestion = "";

        // This symbol is used to represent spaces in the equation to be filled.
        public const string EQUATION_SPACE = "$";

        // The number of values that will be missing from the equation.
        public int missingValueCount = 1;

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

                        // Expression: a^(-1) = 1/(a^n)
                        // Generates number 1 and number 2.
                        int num1 = Random.Range(lowestValue, highestValue + 1); // a (base)
                        int num2 = Random.Range(lowestValue, highestValue + 1); // b (exponent)

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


        // Generates an equation to be filled.
        public void GenerateEquation()
        {
            // TODO: do equation calculation here.
            // Don't have spaces.
            
            // equation = "3x^2*3x^(21)=3x^4"; // Test
            equation = GenerateCalculationAsString(exponentRule.expo, 1, 9, 1, 5);

            // Set the equation qustion.
            equationQuestion = equation;

            // The number of values to replace.
            int replaceCount = (missingValueCount > 0) ? missingValueCount : 0;

            
            // FINDING THE INDEXES
            // The list of indexes in the equation to choose from.
            List<int> usableIndexes = new List<int>();

            // The selected indexes.
            List<int> replaceIndexes = new List<int>();

            // Adds each index to the list.
            for(int i = 0; i < equation.Length; i++)
            {
                // If the index is not an exponent symbol, a bracket, or an equals sign.
                if (equation[i] != '^' && equation[i] != '(' && equation[i] != ')' && equation[i] != '=')
                    usableIndexes.Add(i);
            }

            // Finds what values will be replaced.
            for(int i = 0; i < replaceCount && usableIndexes.Count != 0; i++) 
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
            for(int i = replaceIndexes.Count - 1; i >= 0; i--)
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
                equationQuestion.Remove(vs.index, 1);
                equationQuestion.Insert(vs.index, vs.value.ToString());
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
            
            // Returns the result.
            return result;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}