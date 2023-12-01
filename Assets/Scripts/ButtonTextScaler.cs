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
        [Header("Button")]

        // The button.
        public Button button;

        public int buttonWidth = 1;

        public int buttonHeight = 1;

        [Header("Text")]
        // The text.
        public TMP_Text text;

        [Header("Settings")]
        public bool adjustWidth = true;

        public bool adjustHeight = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Scales the text (TODO: implement)
        public void ScaleText()
        {
            // float bw = buttonWidth * button.transform.localScale.x;
            // float bh = buttonHeight * button.transform.localScale.y;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}