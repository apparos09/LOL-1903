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

        // The power entries. // TODO: change this to an array.
        public PowerEntry[] powerEntryList = new PowerEntry[Power.POWER_TYPE_COUNT];

        // The entry page index (the item at the index is the first one displayed).
        [Tooltip("The entry page index. The entry at this index is the first one in the display.")]
        public int entryPageIndex = -1;
        
        // The selected index.
        public int selectedEntryIndex = -1;

        // The selected entry.
        public PowerEntry selectedEntry;


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

            // Power Info
            if (powerSymbols == null)
                powerSymbols = PowerInfo.Instance;

            // Player World
            if (playerWorld == null)
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
            // Selected entry index.
            selectedEntryIndex = 0;

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
                    entry.name = PowerInfo.GetPowerTypeName(entry.power, true);
                    entry.description = PowerInfo.GetPowerTypeDescription(entry.power);

                    // Saves the entry.
                    powerEntryList[i] = entry;
                }
                else
                {
                    // Set the none entry.
                    powerEntryList[i] = GeneratePowerNoneEntry();

                }

                // If the player's power is this entry, set the index and selected power.
                if (powerEntryList[i].power == playerWorld.power)
                {
                    selectedEntryIndex = i;
                    selectedEntry = powerEntryList[i]; // TODO: check that this works properly.
                }
                    
            }

            // TODO: do more

            // Loads the entries into the UI.
            entryPageIndex = 0;
            LoadEntriesIntoUI();
        }

        // Loads the entries into the UI.
        public void LoadEntriesIntoUI()
        {
            // Makes sure index is valid.
            entryPageIndex = Mathf.Clamp(entryPageIndex, 0, powerEntryList.Length - 1);

            // Puts the entries into an array.
            PowerMenuEntryUI[] entryArr = new PowerMenuEntryUI[3] { powerMenuEntry0, powerMenuEntry1, powerMenuEntry2 };

            // The current index.
            int currIndex = entryPageIndex;

            // Goes through each index.
            foreach(PowerMenuEntryUI entryUI in entryArr) 
            {
                // Activate the object.
                if(!entryUI.gameObject.activeSelf)
                    entryUI.gameObject.SetActive(true);

                // If the index is valid, load the entry.
                if (currIndex >= 0 && currIndex < powerEntryList.Length)
                {
                    entryUI.SetEntry(powerEntryList[currIndex]);
                }
                else // Invalid, so clear entry.
                {
                    // Clear the entry.
                    entryUI.ClearEntry();

                    // Hide the object since it won't be used.
                    entryUI.gameObject.SetActive(false);
                }

                // Increase the index.
                currIndex++;              
            }

        }

        // Sets thei ndex and loads the entries.
        public void LoadEntriesIntoUI(int index)
        {
            entryPageIndex = index;
            LoadEntriesIntoUI();
        }

        // Goes to the previous page.
        public void PreviousPage()
        {
            int index = entryPageIndex - 3;

            // Bounds check.
            if (index < 0)
                index = powerEntryList.Length - 1;

            // Load the entries.
            LoadEntriesIntoUI(index);
        }

        // Goe to the next page.
        public void NextPage()
        {
            int index = entryPageIndex + 3;

            // Bounds check.
            if (index > powerEntryList.Length)
                index = 0;

            // Load the entries.
            LoadEntriesIntoUI(index);
        }

        // Set the selected entry.
        public void SetSelectedEntry(PowerMenuEntryUI entryUI)
        {
            // Set the index.
            int entryIndex = -1;

            // Checks each entry to see which one the provided entry is.
            if(powerMenuEntry0 == entryUI)
            {
                entryIndex = 0;
            }
            else if(powerMenuEntry1 == entryUI)
            {
                entryIndex = 1;
            }
            else if(powerMenuEntry2 == entryUI)
            {
                entryIndex = 2;
            }

            // If the entry index is greater than 0, set the index.
            // The page index aligns with the first entry in the list.
            if(entryIndex > 0)
                entryIndex = entryPageIndex + entryIndex;

            // Sets the selected entry index.
            selectedEntryIndex = entryIndex;

            // Selects this entry.
            selectedEntry = entryUI.entry;
        }

        // Equips the selected power.
        public void EquipPower()
        {
            // The selected entry.
            playerWorld.SetPower(selectedEntry.power);
        }

        // Unequips the current power.
        public void UnequipPower()
        {
            playerWorld.RemovePower();
        }

        //// Update is called once per frame
        //void Update()
        //{
        //    // ...
        //}
    }
}