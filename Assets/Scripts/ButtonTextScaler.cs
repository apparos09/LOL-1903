using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // Dynamically scales the button's text so that it's always within the button.
    // Make sure the TMP settings are correct for this to work.
    // This won't be completed and implemented until the next build.
    public class ButtonTextScaler : MonoBehaviour
    {
        // The button.
        public Button button;

        // The text.
        public TMP_Text text;

        public bool adjustWidth = true;

        public bool adjustHeight = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Scales the text (TODO: implement)
        public void ScaleText()
        {
            // TODO: figure out how to get button width and height.
            float buttonWidth = 0;
            float buttonHeight = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}