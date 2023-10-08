using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The UI for the challenger.
    public class ChallengeUI : MonoBehaviour
    {
        // The world manager.
        public WorldManager manager;

        // The challenger the challenge is being issued by.
        public ChallengerWorld challenger;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = WorldManager.Instance;
        }

    }
}