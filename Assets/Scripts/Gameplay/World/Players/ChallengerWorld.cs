using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // A challenger that's encountered in the game world.
    // The 'World' part is added to make it clearer that it belongs to the world area, not the match area.
    public class ChallengerWorld : MonoBehaviour
    {
        // World manager.
        public WorldManager manager;

        // The difficulty of the challenger.
        public int difficulty = 0;

        [Header("Exponents")]
        // The exponent rates.
        public float baseExpoRate = 1.0F;

        // Rate for multiplicaton (same bases) exponents.
        public float multSameRate = 1.0F;

        // Rate for exponent by exponent exponents.
        public float expoByExpoRate = 1.0F;

        // Rate for multplication (different bases) exponents.
        public float multDiffRate = 1.0F;

        // Rate for zero exponents.
        public float zeroRate = 1.0F;

        // Rate for negative exponents.
        public float negativeRate = 1.0F;


        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = WorldManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}