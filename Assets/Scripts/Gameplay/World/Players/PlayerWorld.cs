using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace RM_EM
{
    // The player in the world scene.
    public class PlayerWorld : MonoBehaviour
    {
        // The match manager.
        public WorldManager manager;

        // The power the player has.
        public Power.powerType power;

        // The list of powers the player has.
        public List<Power.powerType> powerList = new List<Power.powerType>();

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = WorldManager.Instance;
        }

        // Selects a power by type.
        public void SelectPower(Power.powerType powerType)
        {
            // Checks if the player's has the power in their list.
            if(!powerList.Contains(powerType) && powerType != Power.powerType.none) 
            {
                powerList.Add(powerType);
            }

            // Set the power.
            power = powerType;
        }

        // Sorts the power list.
        public void SortPowerList()
        {
            // Sorts the power list.
            if (powerList.Count > 0)
                powerList.Sort();
        }
    }
}