using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

namespace RM_EM
{
    // The mechanic for the sliding puzzle.
    public class SlidingMechanic : SpawnMechanic
    {
        [Header("Sliding")]

        // The top left corner of the sliding area.
        public GameObject topLeftCorner;

        // The bottom right corner of the sliding area.
        public GameObject bottomRightCorner;

        // The number of segments for the sliding area.
        public int segments = 1;

        // The number of pieces in each segment.
        [Tooltip("The number of pieces for each segment. The segment numbering starts at 0.")]
        public List<int> segmentPieceCounts = new List<int>();

        [Header("Sliding/Pieces")]

        // The pool of chips for the puzzle mechanic.
        public Queue<SlidingPiece> piecePool;

        // The prefab of the piece to be instanted.
        public SlidingPiece piecePrefab;

        // Sprites for the sliding mechanic puzzle.
        public PuzzleValueSprites valueSprites;

        // The piece movement direction.
        public Vector3 pieceDirec = new Vector3(0, 1, 0);

        // If 'true', the pieces alternate directions based on the segment they fall into.
        public bool alternateDirec = true;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Add a count for each segment.
            for (int i = 0; i < segments; i++)
                segmentPieceCounts.Add(0);
        }

        // Generates a piece to be used in the sliding area.
        // The piece is given the value provided.
        // The 'startPos' determines the starting position of the instantiated piece.
        // The 'direc' argument determines the direction of travel.
        public SlidingPiece GeneratePiece(char value, int segment, Vector3 startPos, Vector3 direc)
        {
            // The piece being generated.
            SlidingPiece piece;

            // Checks if there are pieces in the pool.
            if(piecePool.Count == 0)
            {
                // The new piece, which is generated from a prefab.
                piece = Instantiate(piecePrefab);
            }
            else // No pieces, so make a new one.
            {
                piece = piecePool.Dequeue();
            }

            // Set the piece value and changes the sprite.
            piece.puzzleValue.SetValueAndSprite(value, valueSprites);

            // Saves the segment the piece is for, and increases the piece count.
            piece.segment = segment;
            segmentPieceCounts[segment]++;

            // Set the position and movement.
            piece.transform.position = startPos;
            piece.moveDirec = direc;

            // Add this to the puzzle value list.
            puzzleValues.Add(piece.puzzleValue);

            // Returns the piece.
            return piece;
        }

        // Returns a piece to the pool when its gone off-screen.
        public void ReturnPiece(SlidingPiece piece)
        {
            // Reduce segment's piece count and set piece segment to -1 (no segment).
            segmentPieceCounts[piece.segment]--;
            piece.segment = -1;

            // Put the piece into the pool.
            piece.gameObject.SetActive(false);
            piecePool.Enqueue(piece);
            
            // Remove this from the puzzle value list.
            puzzleValues.Remove(piece.puzzleValue);
        }
        
        // Returns 'true' if the position is within the bounds of the puzzle space.
        // Only the x and y are checked because the z doesn't matter.
        public bool PositionXYInBounds(Vector3 pos)
        {
            // Checks the x and the y.
            bool inX = pos.x >= topLeftCorner.transform.position.x && pos.x <= bottomRightCorner.transform.position.x;
            bool inY = pos.y >= bottomRightCorner.transform.position.y && pos.y <= topLeftCorner.transform.position.y;

            return inX && inY;
        }
        
        // Updates the mechanic.
        public override void UpdateMechanic()
        {
            // Updates the spawn timer.
            bool spawn = UpdateSpawnTimer();

            // Checks if the values have been maxed out.
            bool maxedValues = HasPuzzleValuesCountReachedMax();

            // Checks if a piece should be generated.
            bool generate = spawn && !maxedValues;

            // Generates a piece.
            if(generate)
            {
                // SEGMENT DECISION //

                // The segment the piece will go in.
                int segment = -1;

                // Gets the number of active pieces.
                int activePieces = GetActivePuzzleValues();

                // Checks how many segments there are. If there's only 1, all the pieces go in the same segment.
                if (segments > 1)
                {
                    // The chance rates for each segment.
                    // The more pieces a segment has, the less likely that segment is to be chosen.
                    float[] rates = new float[segments];

                    // The sum of the all rates.
                    float rateAllSum = 0.0F;

                    // Goes through all the segments.
                    for (int i = 0; i < segments; i++)
                    {
                        // Calculates the chance rate of this segment being chosen.
                        // The lower the number of pieces in the segment, the more likely it is to be chosen.
                        rates[i] = 1.0F - segmentPieceCounts[i] / activePieces;

                        rateAllSum += rates[i];
                    }

                    // Determines the segment.
                    if(rateAllSum > 0) // There are options.
                    {
                        // Generates a random value.
                        float randValue = Random.Range(0, rateAllSum);

                        // bool rate = false;
                        // Used to sum of the rates.
                        float rateSum = 0.0F;

                        // Goes through each rate...
                        for(int i = 0; i < rates[i]; i++)
                        {
                            // If the rate plus the current rate sum is less than the rand value...
                            // Add to the rate sum and move onto the next one.
                            if (rateSum + rates[i] < randValue)
                            {
                                // Increase the sum.
                                rateSum += rates[i];
                            }
                            else // Pick the segment since the randValue falls into this range.
                            {
                                // Set the segment.
                                segment = i;
                                break;
                            }
                        }
                    }
                    else // All have the same chance, so just choose at random.
                    {
                        segment = Random.Range(0, segments);
                    }
                    
                }
                else // Only one segment.
                {
                    segment = 0;
                }


                // POSITIONING //

                // Calculate where the piece will be placed.
                // First, get the width and height of the area.
                float width = Mathf.Abs(bottomRightCorner.transform.position.x - topLeftCorner.transform.position.x);
                float height = Mathf.Abs(topLeftCorner.transform.position.y - bottomRightCorner.transform.position.y);

                // The position increment for the width and height.
                // Segments is increased by '1' because the pieces go along the dividing lines in the puzzle space.
                Vector3 posInc = new Vector3(width / (segments + 1), height / (segments + 1), 0.0F);

                // Gets set to 'true' if the direction should be flipped.
                bool flipDirec = (alternateDirec) ? segment % 2 == 0 : false;

                // The movement direction of the piece.
                Vector3 moveDirec = (flipDirec) ? pieceDirec * -1 : pieceDirec;

                // The x and y sides the piece STARTS on.
                int xSide = 0;
                int ySide = 0;

                // Checks the movement direction to know where the piece should be placed.
                // The y-takes priority since it starts from the top by default.
                if(moveDirec.y != 0) // Y-only
                {
                    ySide = (moveDirec.y > 0) ? -1 : 1;
                }
                else if (moveDirec.x != 0) // X-only
                {
                    xSide = (moveDirec.x > 0) ? -1 : 1;
                }
                else // Both are zero, or both are set (there's a problem).
                {
                    // Problem.
                }

                // Calculates the piece's position. The positioning is based around the top left corner of the area.
                // The z-position shouldn't change.
                // Since segment 0 is used, segment + 1 is done to make sure the piece isn't on the edge of the border.
                Vector3 piecePos = topLeftCorner.transform.position + 
                    (new Vector3(posInc.x * xSide, -posInc.y * ySide, posInc.z) * (segment + 1));


                // Generates the piece.
                GeneratePiece(puzzle.GetRandomPuzzleValue(), segment, piecePos, moveDirec);

                // Resets the spawn timer.
                ResetSpawnTimerToMax();
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}