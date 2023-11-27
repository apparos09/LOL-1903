using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EM
{
    // The tutorial class. This is a singleton that gets gets consulted for tutorial text.
    public class Tutorial : MonoBehaviour
    {
        [System.Serializable]
        public struct TutorialData
        {
            // Bools for clearing certain tutorials.
            public bool clearedOpening;
            public bool clearedFirstMatchLoss;
            public bool clearedFirstMatchWin;

            public bool clearedFirstPower;
            public bool clearedFinalMatch;
            public bool clearedGameOver;

            // Exponents
            public bool clearedExponent;
            public bool clearedProduct;
            public bool clearedPowerOfAPower;
            public bool clearedProductOfAProduct;

            public bool clearedZero;
            public bool clearedNegative;
        }

        // The singleton instance.
        private static Tutorial instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The definitions from the json file.
        private JSONNode defs;

        [Header("Triggers")]
        
        // TODO: maybe seperate them based on the tutorial type (world, match). Maybe re-arrange the function.
        // Bools for clearing certain tutorials.
        public bool clearedOpening = false;
        public bool clearedFirstMatchLoss = false;
        public bool clearedFirstMatchWin = false;

        public bool clearedFirstPower = false;
        public bool clearedFinalMatch = false;
        public bool clearedGameOver = false;

        [Header("Triggers/Exponents")]
        public bool clearedExponent = false;
        public bool clearedProduct = false;
        public bool clearedPowerOfAPower = false;
        public bool clearedPowerOfAProduct = false;

        public bool clearedZero = false;
        public bool clearedNegative = false;

        // Constructor
        private Tutorial()
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
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;

                // Loads the language definitions.
                defs = SharedState.LanguageDefs;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Don't destroy this game object on load.
            DontDestroyOnLoad(gameObject);
        }

        // Gets the instance.
        public static Tutorial Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<Tutorial>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Tutorial (singleton)");
                        instance = go.AddComponent<Tutorial>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Gets the test pages.
        public List<Page> GetTestPages()
        {
            // The test pages.
            List<Page> pages = new List<Page>()
            {
                new Page("This is a test."),
                new Page("This is only a test.")
            };

            // Returns the pages.
            return pages;
        }

        // The opening tutorial.
        public List<Page> GetOpeningTutorial()
        {
            // Pages
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_opening_00"], "trl_opening_00"));
                pages.Add(new Page(defs["trl_opening_01"], "trl_opening_01"));
            }
            else
            {
                pages.Add(new Page("Welcome to the exponent club! As the name suggests, we play the exponent game here. Do you want to play? Oh, you’ve never played the game before? I guess the club will have to teach you then! Challenge me to a match, and I’ll show you the ropes!"));
                pages.Add(new Page("The exponent game is simple. You and your opponent will both be given equations with missing values, which can be numbers or operations. You need to pick the right value from your puzzle board to complete the equation. When the equation is complete, you get points, and move onto the next equation. The player fills their points bar first wins. With all that explained, challenge me to a match by selecting my icon!"));
            }
            

            clearedOpening = true;

            return pages;
        }

        // The exponent basics tutorial.
        public List<Page> GetExponentTutorial()
        {
            // Pages
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_exponent_00"], "trl_exponent_00"));
            }
            else
            {
                pages.Add(new Page("Time to teach you some exponent basics. Exponents are math operations where you multiply a value by itself a certain number of times, with said number being determined by the exponent. Exponent operations have multiple rules, which will be explained later."));
            }


            // If the world manager is instantiated.
            if(WorldManager.Instantiated)
            {
                // Gets the instance.
                WorldManager manager = WorldManager.Instance;

                // Makes the info button interactable.
                pages[0].OnPageClosedAddCallback(manager.worldUI.SetInfoButtonInteractable);
            }
            

            // Cleared the tutorial.
            clearedExponent = true;

            return pages;
        }

        // The first match loss tutorial.
        public List<Page> GetFirstMatchLossTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_firstMatchLoss_00"], "trl_firstMatchLoss_00"));
            }
            else
            {
                pages.Add(new Page("You’ve lost the introductory match. It’s important that you learn this rule before going forward, so please try again."));
            }

            clearedFirstMatchLoss = true;

            return pages;
        }

        // The first match win/post-first match tutorial.
        public List<Page> GetFirstMatchWinTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_firstMatchWin_00"], "trl_firstMatchWin_00"));
            }
            else
            {
                pages.Add(new Page("Congratulations on winning the introduction match! Now that you’re familiar with the rules, you’ll have to beat every member of the club! Each member focuses on one or more exponent rules, so you’ll get introduced to each rule one at a time. You’ll be a master at this in no time. Good luck!"));
            }

            clearedFirstMatchWin = true;

            return pages;
        }

        // The product rule tutorial.
        public List<Page> GetProductRuleTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_product_00"], "trl_product_00"));
            }
            else
            {
                pages.Add(new Page("This match uses the product rule. When two terms with the same base are multiplied together, it can be simplified to multiplying the base by the sum of the exponents."));
            }

            clearedProduct = true;

            return pages;
        }

        // The power of a power rule tutorial.
        public List<Page> GetPowerOfAPowerRuleTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_powerOfAPower_00"], "trl_powerOfAPower_00"));
            }
            else
            {
                pages.Add(new Page("This match uses the power of a power rule. When a term with an exponent is having its result get another exponent applied to it, it can be converted to the base value to a single, combined exponent. The combined exponent is the two exponents from the original expression multiplied together."));
            }

            clearedPowerOfAPower = true;

            return pages;
        }

        // The product of a product rule tutorial.
        public List<Page> GetPowerOfAProductRuleTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_powerOfAProduct_00"], "trl_powerOfAProduct_00"));
            }
            else
            {
                pages.Add(new Page("This match uses the product of a product rule. If two terms with different bases and the same exponents are multiplied, the bases are multiplied together, with their result being put to the power of the shared exponent."));
            }

            clearedPowerOfAProduct = true;

            return pages;
        }

        // The zero exponent rule tutorial.
        public List<Page> GetZeroExponentRuleTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_zero_00"], "trl_zero_00"));
            }
            else
            {
                pages.Add(new Page("This match uses the zero exponent rule. Any value to the power of 0 will give the result of 1."));
            }

            clearedZero = true;

            return pages;
        }

        // The negative exponent rule tutorial.
        public List<Page> GetNegativeExponentRuleTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_negative_00"], "trl_negative_00"));
            }
            else
            {
                pages.Add(new Page("This match uses the negative exponent rule. The negative exponent rule dictates that any value to the power of a negative exponent becomes a fraction with a numerator of 1. The value with the exponent applied becomes the denominator."));
            }

            clearedNegative = true;

            return pages;
        }

        // The first power tutorial.
        public List<Page> GetFirstPowerTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_firstPower_00"], "trl_firstPower_00"));
            }
            else
            {
                pages.Add(new Page("You just got a match power! A power can be used to give you an edge during a match, but only after building up its energy first. When you defeat a challenger, you also receive the power, which you can equip for future matches. Check the power menu to see what each power does, and select what power you want."));
            }

            clearedFirstPower = true;

            return pages;
        }

        // The final match tutorial.
        public List<Page> GetFinalMatchTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_finalMatch_00"], "trl_finalMatch_00"));
            }
            else
            {
                pages.Add(new Page("You’ve learned all the exponent rules and defeated all the other club members, so now it’s time for your final challenge: a match against me! Head over to the next room so we can face off!"));
            }

            clearedFinalMatch = true;

            return pages;
        }

        // The game over tutorial.
        public List<Page> GetGameOverTutorial()
        {
            // The pages list.
            List<Page> pages = new List<Page>();

            // Loads the pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_gameOver_00"], "trl_gameOver_00"));
            }
            else
            {
                pages.Add(new Page("You got a game over. You can rematch your opponent now, or try to take on any other available challengers. However, you’ll need to beat all the club members to clear this trial, so be ready to come back at some point."));
            }

            clearedGameOver = true;

            return pages;
        }


        // Generates tutorial data.
        public TutorialData GenerateTutorialData()
        {
            TutorialData data = new TutorialData();

            // Saving the data
            data.clearedOpening = clearedOpening;
            data.clearedFirstMatchLoss = clearedFirstMatchLoss;
            data.clearedFirstMatchWin = clearedFirstMatchWin;

            data.clearedFirstPower = clearedFirstPower;
            data.clearedFinalMatch = clearedFinalMatch;
            data.clearedGameOver = clearedGameOver;

            data.clearedExponent = clearedExponent;
            data.clearedProduct = clearedProduct;
            data.clearedPowerOfAPower = clearedPowerOfAPower;
            data.clearedProductOfAProduct = clearedPowerOfAProduct;

            data.clearedZero = clearedZero;
            data.clearedNegative = clearedNegative;

            return data;
    }

        // Loads tutorial data.
        public void LoadTutorialData(TutorialData data)
        {
            // Loading the data
            clearedOpening = data.clearedOpening;
            clearedFirstMatchLoss = data.clearedFirstMatchLoss;
            clearedFirstMatchWin = data.clearedFirstMatchWin;

            clearedFirstPower = data.clearedFirstPower;
            clearedFinalMatch = data.clearedFinalMatch;
            clearedGameOver = data.clearedGameOver;

            clearedExponent = data.clearedExponent;
            clearedProduct = data.clearedProduct;
            clearedPowerOfAPower = data.clearedPowerOfAPower;
            clearedPowerOfAProduct = data.clearedProductOfAProduct;

            clearedZero = data.clearedZero;
            clearedNegative = data.clearedNegative;
        }


        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}