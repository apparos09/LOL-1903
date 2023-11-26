using System.Collections;
using System.Collections.Generic;
using TMPro;
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

            // Symbol
            public Sprite symbol;

            // Name
            public string name;
            // TODO: add shorthand name...

            // Description
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

        // The power entries the player can select.
        public List<PowerEntry> powerEntryList = new List<PowerEntry>();

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

        // The power menu UI entry count.
        public const int POWER_MENU_ENTRY_UI_COUNT = 3;

        // The previous entry button.
        public Button prevEntryButton;

        // The next entry button.
        public Button nextEntryButton;

        // The button for equipping powers.
        public Button equipButton;

        [Header("Entries (UI)/Selected")]

        // The selected power symbol.
        public Image selectedPowerSymbol;

        // The selected power name.
        public TMP_Text selectedPowerName;

        // The selected power description.
        public TMP_Text selectedPowerDesc;

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
            entry.symbol = PowerInfo.Instance.nothingSymbol;

            // Gets the name and the description.
            entry.name = PowerInfo.GetPowerTypeName(entry.power, false);
            entry.description = PowerInfo.GetPowerTypeDescription(entry.power);

            return entry;
        }

        // Loads the player power list.
        public void LoadPlayerPowerList()
        {
            // Selected entry index.
            selectedEntryIndex = 0;

            // Sorts the power list.
            playerWorld.SortPowerList();

            // Clears the list.
            powerEntryList.Clear();

            // Generates a none entry as the first spot (used to deselect powers).
            // TODO: remove this?
            powerEntryList.Add(GeneratePowerNoneEntry());

            // Default to first index (nothing power).
            selectedEntryIndex = 0;

            // Goes through all of the player's powers.
            for (int i = 0; i < playerWorld.powerList.Count; i++)
            {
                // New entry.
                PowerEntry entry = new PowerEntry();

                // Saves the power information
                // Power and Symbol
                entry.power = playerWorld.powerList[i];
                entry.symbol = PowerInfo.Instance.GetPowerSymbol(entry.power);

                // Name and Description
                entry.name = PowerInfo.GetPowerTypeName(entry.power, true);
                entry.description = PowerInfo.GetPowerTypeDescription(entry.power);
                

                // Adds the entry to the list.
                powerEntryList.Add(entry);

                // If this is the player's current power, set this as the selected power.
                if (playerWorld.powerList[i] == playerWorld.power)
                {
                    selectedEntryIndex = powerEntryList.Count - 1;
                    selectedEntry = powerEntryList[i];
                }
            }

            // TODO: do more

            // Sets to the first page.
            entryPageIndex = 0;

            // TODO: use selected entry instead?
            // Set selected power symbol, name, description to current.
            // If not available, set to defaults.
            if(playerWorld.power != Power.powerType.none)
            {
                selectedPowerSymbol.sprite = PowerInfo.Instance.GetPowerSymbol(playerWorld.power);
                selectedPowerName.text = PowerInfo.GetPowerTypeName(playerWorld.power, false);
                selectedPowerDesc.text = PowerInfo.GetPowerTypeDescription(playerWorld.power);
            }
            else // None power
            {
                selectedPowerSymbol.sprite = powerEntryList[0].symbol;
                selectedPowerName.text = powerEntryList[0].name;
                selectedPowerDesc.text = powerEntryList[0].description;
            }
            

            // Make buttons interactable.
            prevEntryButton.interactable = true;
            nextEntryButton.interactable = true;

            // If there are no other pages...
            if (powerEntryList.Count <= 3)
            {
                prevEntryButton.interactable = false;
                nextEntryButton.interactable = false;
            }

            // Loads the entry into the UI.
            LoadEntriesIntoUI(0);

            // TODO: disable the equip button if the player has no power equipped.
            // It doesn't work when the player first opens the menu, so I need to fix that.
        }

        // Loads the entries into the UI.
        public void LoadEntriesIntoUI()
        {
            // Makes sure index is valid.
            entryPageIndex = Mathf.Clamp(entryPageIndex, 0, powerEntryList.Count - 1);

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
                if (currIndex >= 0 && currIndex < powerEntryList.Count)
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

                // Reset the currIndex variable.
                if (currIndex >= powerEntryList.Count)
                    currIndex = 0;
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
            // If there's 3 entries or less (i.e., all of them can appear on screen)...
            // Then don't allow page switching.
            if (powerEntryList.Count <= POWER_MENU_ENTRY_UI_COUNT)
                return;

            // Shift by -1.
            int index = entryPageIndex - 1;

            // Bounds check.
            if (index < 0)
                index = powerEntryList.Count - 1;

            // Load the entries.
            LoadEntriesIntoUI(index);
        }

        // Goe to the next page.
        public void NextPage()
        {
            // If there's 3 entries or less (i.e., all of them can appear on screen)...
            // Then don't allow page switching.
            if (powerEntryList.Count <= POWER_MENU_ENTRY_UI_COUNT)
                return;

            // Shift by +1.
            int index = entryPageIndex + 1;

            // Bounds checks.
            if (index >= powerEntryList.Count)
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
            selectedEntry.power = entryUI.entry.power;

            // Set the symbol.
            selectedPowerSymbol.sprite = selectedEntry.symbol;

            // The selected power's name.
            selectedPowerName.text = selectedEntry.name;
            selectedPowerDesc.text = selectedEntry.description;


            // Set button to be interactable.
            equipButton.interactable = true;

            // If the player has this power, disable the equip button.
            if(playerWorld.power == selectedEntry.power)
            {
                equipButton.interactable = false;
            }
        }

        // Equips the selected power.
        public void EquipPower()
        {
            // The selected entry.
            playerWorld.SetPower(selectedEntry.power);

            // Sets the equip button.
            equipButton.interactable = true;

            // If the power was selected, disable the equip button.
            if(playerWorld.power == selectedEntry.power)
            {
                equipButton.interactable = false;
            }
        }

        // Unequips the current power.
        public void UnequipPower()
        {
            playerWorld.RemovePower();

            // Sets to the 'none' power.
            // TODO: streamline this
            selectedPowerSymbol.sprite = powerEntryList[0].symbol;
            selectedPowerName.text = powerEntryList[0].name;
            selectedPowerDesc.text = powerEntryList[0].description;
        }

        //// Update is called once per frame
        //void Update()
        //{
        //    // ...
        //}
    }
}