using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The power prefabs.
    public class PowerInfo : MonoBehaviour
    {
        // The singleton instance.
        private static PowerInfo instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private bool instanced = false;

        [Header("Prefabs")]

        // The nothing power (for testing purposes only).
        public PowerNothing nothing;

        // Increases the number of points the user gets.
        public PowerPoints pointsPlus;

        // Decreases the number of points the target gets.
        public PowerPoints pointsMinus;

        // Shortens the equation.
        public PowerEquationChange equationShorten;

        // Lengths an equation.
        public PowerEquationChange equationLengthen;

        // Transfers points from one player to another.
        public PowerPointsTransfer pointsTransfer;

        // Twists the opponent's render (no use for the player since the AI isn't effected).
        public PowerTwist twist;

        [Header("Symbols")]

        // The default power symbol.
        public Sprite defaultSymbol;

        // The none/nothing symbol.
        public Sprite noneSymbol;

        // The points plus symbol.
        public Sprite pointsPlusSymbol;

        // The points minus symbol.
        public Sprite pointsMinusSymbol;

        // The equation shorten symbol.
        public Sprite equationShortenSymbol;

        // The equation lengthn symbol.
        public Sprite equationLengthenSymbol;

        // The points transfer symbol.
        public Sprite pointsTransferSymbol;

        // The render twist symbol.
        public Sprite twistSymbol;

        // Constructor
        private PowerInfo()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Gets the instance.
        public static PowerInfo Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<PowerInfo>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Power Prefabs (singleton)");
                        instance = go.AddComponent<PowerInfo>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // INFO
        // Gets the name of the power type name.
        // 'shortHand' determines if the name is the shorthand or the full-name.
        public static string GetPowerTypeName(Power.powerType power, bool shortHand)
        {
            // Sets the name string to be empty.
            string name = string.Empty;

            switch(power)
            {
                case Power.powerType.none:
                    name = "None";
                    break;
                case Power.powerType.pointsPlus:
                    name = "Points Plus";
                    break;
                case Power.powerType.pointsMinus:
                    name = "Points Minus";
                    break;
                case Power.powerType.twist:
                    name = "Twist";
                    break;
            }

            // TODO: implement.
            return name;
        }

        // Gets the power type description.
        public static string GetPowerTypeDescription(Power.powerType power)
        {
            // Sets the description string to be empty.
            string desc = string.Empty;

            switch (power)
            {
                case Power.powerType.none:
                    desc = "A power that does nothing.";
                    break;
                case Power.powerType.pointsPlus:
                    desc = "A power that increases the user's points for a time.";
                    break;
                case Power.powerType.pointsMinus:
                    desc = "A power that decreases the opponent's points for a time.";
                    break;
                case Power.powerType.twist:
                    desc = "A power that flips the opponent's view upside down.";
                    break;
            }

            // TODO: implement.
            return desc;
        }
    }
}