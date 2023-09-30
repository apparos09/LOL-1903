using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EM
{
    // A parent class for managing all gameplay.
    public class GameplayManager : MonoBehaviour
    {
        // The timer for the game.
        public float gameTime = 0;

        // Pauses the timer if true.
        public bool gamePaused = false;

        // The mouse touch object.
        public MouseTouchInput mouseTouch;

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // ...
        }

        // Checks if the game is using the tutorial.
        public bool IsUsingTutorial()
        {
            bool result = GameSettings.Instance.UseTutorial;
            return result;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // The game isn't paused.
            if(!gamePaused)
            {
                gameTime += Time.fixedDeltaTime;
            }
        }
    }
}