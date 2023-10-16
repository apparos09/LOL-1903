using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The bubble mechanic.
    public class BubbleMechanic : PuzzleMechanic
    {
        // The bubble pool.
        public Queue<BubbleValue> bubblePool;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Generates a bubble.
        public void GenerateBubble()
        {
            // ...
        }

        // Returns a bubble to the pool.
        public void ReturnBubble()
        {
            // ...
        }

        // Called when the bubble is to be killed.
        public void OnBubbleKill()
        {
            // TODO: implement.
        }

        // Updates the mechanic.
        public override void UpdateMechanic()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}