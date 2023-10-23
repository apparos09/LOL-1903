using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        // The power symbols.
        public PowerInfo powerSymbols;

        // The player.
        public PlayerWorld playerWorld;

        // The power entries.
        public PowerEntry[] powerEntryList = new PowerEntry[Power.POWER_TYPE_COUNT];

        [Header("Entries (UI)")]
        // Menu entry 0
        public PowerMenuEntryUI powerMenuEntry0;

        // Menu entry 1
        public PowerMenuEntryUI powerMenuEntry1;

        // Menu entry 2
        public PowerMenuEntryUI powerMenuEntry2;

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

        // Generates a power none entry.
        public PowerEntry GeneratePowerNoneEntry()
        {
            PowerEntry entry = new PowerEntry();

            entry.power = Power.powerType.none;

            entry.name = "-";
            entry.description = "-";

            return entry;
        }

        // Loads the player power list.
        public void LoadPlayerPowerList()
        {
            // Sorts the power list.
            playerWorld.SortPowerList();

            // Generates a none entry as the first spot (used to deselect powers).
            powerEntryList[0] = GeneratePowerNoneEntry();

            // Goes through all entries.
            for(int i = 1; i < powerEntryList.Length; i++) 
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
                    powerEntryList[i] = entry;
                }
                else
                {
                    // Set the none entry.
                    powerEntryList[i] = GeneratePowerNoneEntry();

                }
            }

            // TODO: do more

            // Loads the entries into the UI.
            LoadEntriesIntoUI();
        }

        // Loads the entries into the UI.
        public void LoadEntriesIntoUI()
        {
            // TODO: implement.
        }

        // Update is called once per frame
        void Update()
        {
            // ...
        }
    }
}