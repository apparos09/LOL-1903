using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The UI for the power menu.
    public class PowerMenuUI : MonoBehaviour
    {
        // The power entry.
        public struct PowerEntry
        {
            // The power type.
            public Power.powerType power;

            // Name and description.
            public string name;
            public string description;
        }

        // The world manager.
        public WorldManager worldManager;

        // The world UI.
        public WorldUI worldUI;

        // The player.
        public PlayerWorld playerWorld;

        // The power entries.
        public PowerEntry[] powerEntries = new PowerEntry[Power.POWER_TYPE_COUNT];

        // Start is called before the first frame update
        void Start()
        {
            // Manager
            if (worldManager == null)
                worldManager = WorldManager.Instance;

            // World UI
            if (worldUI == null)
                worldUI = worldManager.worldUI;

            // Player World
            if(playerWorld == null)
                playerWorld = worldManager.playerWorld;

            // Loads the player power list.
            LoadPlayerPowerList();
        }

        // OnEnable
        private void OnEnable()
        {
            LoadPlayerPowerList();
        }

        // Loads the player power list.
        public void LoadPlayerPowerList()
        {
            // Sorts the power list.
            playerWorld.SortPowerList();

            // Goes through all entries.
            for(int i = 0; i < powerEntries.Length; i++) 
            { 
                // Put in list if valid.
                if(i < playerWorld.powerList.Count)
                {
                    // New entry.
                    PowerEntry entry = new PowerEntry();

                    // Saves the power information (TODO: call function for name and description).
                    entry.power = playerWorld.powerList[i];
                    entry.name = Power.GetPowerName(entry.power);
                    entry.description = Power.GetPowerDescription(entry.power);

                    // Saves the entry.
                    powerEntries[i] = entry;
                }
                else
                {
                    // Set to null/empty.
                    powerEntries = null;
                }
            }

            // TODO: do more
        }

        // Update is called once per frame
        void Update()
        {
            // ...
        }
    }
}