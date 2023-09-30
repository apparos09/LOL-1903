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

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the instance.
            if (manager == null)
                manager = MatchManager.Instance;
        }

        // TODO: add select value function.

        // Update is called once per frame
        void Update()
        {

        }
    }
}