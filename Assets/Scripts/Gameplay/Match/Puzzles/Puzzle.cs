using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The puzzle types.
    enum puzzle { keypad, slider, bubble, pinball }

    // The exponent types.
    enum exponent { expo, sameMult, expoByExpo, diffMult, zero, negative }

    // Generates content for a puzzle.
    public class Puzzle : MonoBehaviour
    {
        // The manager for the match.
        public MatchManager manager;

        // The renderer for the puzzle space.
        public PuzzleRender puzzleRender;

        // The player this puzzle belongs to.
        public PlayerMatch player;

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
        public Stack<char> missingValues = new Stack<char>();

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance.
            if (manager == null)
                manager = MatchManager.Instance;

            // Generates a equation to start off.
            GenerateEquation();
        }

        // Generates an equation to be filled.
        public void GenerateEquation()
        {
            // TODO: do equation calculation here.
            // Don't have spaces.
            equation = "3x^2*3x^2=3x^4";
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
                missingValues.Push(equationQuestion[replaceIndexes[i]]);

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
            if(value.value == missingValues.Peek())
            {
                Debug.Log("Right!");
            }
            else
            {
                Debug.Log("Wrong!");
            }
        }

        // Gets the question as displayed.
        public string GetDisplayQuestion()
        {

            // TODO: you need to make sure you format it properly for exponents.

            string result = equationQuestion;
            return result;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}