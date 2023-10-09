using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // The UI for the title scene.
    public class TitleUI : MonoBehaviour
    {
        public TitleManager manager;

        [Header("Buttons")]

        // The new game button and continue button.
        public Button newGameButton;
        public Button continueButton;

        // The controls, settings, and credits.
        public Button controlsButton;
        public Button settingsButton;
        public Button creditsButton;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = TitleManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            // ...
        }
    }
}