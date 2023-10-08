using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // Information used to create the game world when coming from the match scene.
    public class WorldInfo : MonoBehaviour
    {
        // The game time.
        public float gameTime = 0;

        // The current room number for the world.
        public int roomNumber = 0;
    }
}