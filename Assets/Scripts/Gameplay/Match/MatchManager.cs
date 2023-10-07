using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RM_EM
{
    // The manager for matches.
    public class MatchManager : GameplayManager
    {
        // The matchamanger instance.
        private static MatchManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        // Gets set to 'true', when post start has been called.
        private bool calledPostStart = false;

        [Header("Match")]

        // The UI for the match.
        public MatchUI matchUI;

        // The point goal for the game.
        public int pointGoal = 999;

        // If 'true', the point goal is used.
        public bool usePointGoal = true;

        // The current time for the match.
        public float matchTime = 0;

        // Checks if the match is paused.
        public bool matchPaused = false;

        // PLAYERS
        [Header("Player 1")]
        // P1
        public PlayerMatch p1;
        // P1 Puzzle
        public Puzzle p1Puzzle;

        // An object used to position P1's mechanic and camera off-screen.
        public GameObject p1MechanicPos;

        [Header("Player 2")]
        // P2
        public PlayerMatch p2;
        // P2 Puzzle
        public Puzzle p2Puzzle;

        // An object used to position P1's mechanic and camera off-screen.
        public GameObject p2MechanicPos;

        // Constructor
        private MatchManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
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

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // matchUI.timeText.text = matchTime.ToString("F2");
            MatchInfo info = FindObjectOfType<MatchInfo>();

            // There's a match info object, so set it up. 
            if(info != null)
            {
                // PUZZLE //
                // Set the puzzle type.
                p1Puzzle.puzzleType = info.puzzleType;
                p2Puzzle.puzzleType = info.puzzleType;

                // Puzzle Mechanics
                // Grabs the instance.
                PuzzlePrefabs puzzlePrefabs = PuzzlePrefabs.Instance;

                // The mechanics for P1 and P2.
                PuzzleMechanic p1Mech, p2Mech;

                // Checks the puzzle type.
                switch (info.puzzleType)
                {
                    // Generates a keypad by default.
                    default:
                    case puzzle.keypad:
                        p1Mech = Instantiate(puzzlePrefabs.keypad);
                        p2Mech = Instantiate(puzzlePrefabs.keypad);
                        break;
                }

                // Change the names.
                p1Mech.name += " (P1)";
                p2Mech.name += " (P2)";

                // Set P1 Puzzle Mechanic Parent and Position
                p1Mech.transform.parent = p1Puzzle.transform; // Parent

                if (p1MechanicPos != null) // Position
                    p1Mech.transform.position = p1MechanicPos.transform.position;


                // Set P2 Puzzle Mechanic Parent and Position
                p2Mech.transform.parent = p2Puzzle.transform; // Parent

                if (p2MechanicPos != null) // Position
                    p2Mech.transform.position = p2MechanicPos.transform.position;

                // Set the managers.
                p1Mech.manager = this;
                p2Mech.manager = this;

                // Set the mechanics for the puzzles, and vice versa.
                // Mechanics
                p1Puzzle.puzzleMechanic = p1Mech;
                p2Puzzle.puzzleMechanic = p2Mech;

                // Puzzles
                p1Mech.puzzle = p1Puzzle;
                p2Mech.puzzle = p2Puzzle;

                // COMPUTER DIFFICULTY //

                // UI/DESIGN //
            }
        }

        // Post start function.
        private void PostStart()
        {
            // Generates a puzzle and displays the equation.
            p1Puzzle.GenerateEquation();
            matchUI.UpdatePlayer1EquationDisplay();
            
            // Generates a puzzle and displays the equation.
            p2Puzzle.GenerateEquation();
            matchUI.UpdatePlayer2EquationDisplay();

            // Called when the equations have been generated.
            p1.OnEquationGenerated();
            p2.OnEquationGenerated();

            calledPostStart = true;
        }

        // Gets the instance.
        public static MatchManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<MatchManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("MatchManager (singleton)");
                        instance = go.AddComponent<MatchManager>();
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

        // SETTINGS //

        // Pauses the match.
        public void PauseMatch()
        {
            // Set paused.
            matchPaused = true;

            // Disable the players.
            p1.enabled = false;
            p2.enabled = false;

            // Set time scale to 0.
            Time.timeScale = 0;
        }

        // Unpauses the match.
        public void UnpauseMatch()
        {
            // Set not paused.
            matchPaused = false;

            // Enable the players.
            p1.enabled = true;
            p2.enabled = true;

            // Set time scale to 1.
            Time.timeScale = 1;
        }

        // Toggles the match being paused.
        public void TogglePausedMatch()
        {
            // Checks if the game is paused or not.
            bool paused = !matchPaused;

            // Calls proper function.
            if (paused)
                PauseMatch();
            else
                UnpauseMatch();
        }


        // MECHANICS //

        // Called when the equation has been answered.
        // player: the player that answered the question.
        // equationFilled: the filled in equation.
        // rulesUsed: the number of rules used for the equation.
        // missingValuesCountStart: the number of blanks to fill in at the start.
        public void OnEquationComplete(Puzzle puzzle, PlayerMatch player, string equationFilled, 
            List<exponentRule> rulesUsed, int missingValuesCountStart)
        {
            // TODO: calculate points.

            // Add points.
            player.points += 1;

            // Called when the equation has been completed.
            player.OnEquationComplete();

            // Generates a new equation if no one has won yet.
            puzzle.GenerateEquation();

            // Called when an equation has been generated.
            player.OnEquationGenerated();

            // Updates the displays.
            if (player == p1)
            {
                matchUI.UpdatePlayer1EquationDisplay();
                matchUI.UpdatePlayer1PointsBar();
            }
            else if (player == p2)
            {
                matchUI.UpdatePlayer2EquationDisplay();
                matchUI.UpdatePlayer2PointsBar();
            }
                
            // Checks if the player has won. If so, call the game finished function.
            if(HasPlayerWon(player))
            {
                OnGameFinished();
            }
        }

        // Checks if the provided player has won.
        public bool HasPlayerWon(PlayerMatch player)
        {
            bool result = player.points >= pointGoal;
            return result;
        }

        // Called when the game is finished.
        public void OnGameFinished()
        {
            // Gets set to 'true' if p1 has one.
            PlayerMatch winner = null;

            // Checks if p1 has reached the points goal.
            if(p1.points >= pointGoal)
            {
                winner = p1;
            }
            // Checks if p2 has reached the points goal.
            else if(p2.points >= pointGoal)
            {
                winner = p2;
            }

            // The winner couldn't be set.
            if(winner == null)
            {
                Debug.LogWarning("The winner cannot be found.");
                return;
            }

            // Pause the match to stop player inputs and AI calculations.
            PauseMatch();

            // Show the match end content.
            matchUI.ShowMatchEnd();
        }


        // Update is called once per frame
        protected override void Update()
        {
            // Calls post start if it hasn't been called yet.
            if (!calledPostStart)
                PostStart();

            base.Update();

            // The match isn't paused.
            if (!matchPaused)
            {
                matchTime += Time.fixedDeltaTime;
            }
        }
    }
}