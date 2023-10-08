using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The World UI
    public class WorldUI : MonoBehaviour
    {
        // The match manager.
        public WorldManager manager;

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