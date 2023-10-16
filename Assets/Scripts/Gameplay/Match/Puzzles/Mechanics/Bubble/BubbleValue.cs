using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The bubble piece.
    public class BubbleValue : MonoBehaviour
    {
        // The bubble mechanic.
        public BubbleMechanic mechanic;

        // The bubble value's rigidbody.
        public new Rigidbody2D rigidbody;

        // How long the bubble is alive for.
        public float lifeTimer = 10.0F;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the rigidbody.
            if(rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();
        }

        // Kills the bubble.
        public void Kill()
        {
            mechanic.OnBubbleKill();
        }

        // Update is called once per frame
        void Update()
        {
            // If the match isn't paused.
            if(!mechanic.manager.MatchPaused)
            {
                // Reduce timer.
                lifeTimer -= Time.deltaTime;

                // Set life timer to 0 if negative.
                if (lifeTimer < 0)
                    lifeTimer = 0;
            
                // If the life timer is set to 0.
                if(lifeTimer == 0)
                {
                    // Kills the bubble.
                    Kill();
                }
            }
        }
    }
}