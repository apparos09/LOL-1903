using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The keyboard mechanic.
    public class KeypadMechanic : PuzzleMechanic
    {
        [Header("Keyboard")]
        // Numbers.
        public PuzzleValue key0;
        public PuzzleValue key1;
        public PuzzleValue key2;
        public PuzzleValue key3;
        public PuzzleValue key4;
        public PuzzleValue key5;
        public PuzzleValue key6;
        public PuzzleValue key7;
        public PuzzleValue key8;
        public PuzzleValue key9;        

        // Math operations.
        public PuzzleValue keyPlus;
        public PuzzleValue keyMinus;
        public PuzzleValue keyMultiply;
        public PuzzleValue keyDivide;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Set the puzzle type.
            puzzle.puzzle = Puzzle.puzzleType.keypad;
        }

        // Updates the mechanic.
        public override void UpdateMechanic()
        {
            // Nothing
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}