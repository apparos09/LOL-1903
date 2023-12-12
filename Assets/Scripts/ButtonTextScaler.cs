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

        // The button rect.
        public RectTransform buttonRect;

        public float buttonWidth = 1;

        public float buttonHeight = 1;

        [Header("Text")]
        // The text.
        public TMP_Text text;

        [Header("Settings")]
        public bool adjustWidth = true;

        public bool adjustHeight = true;

        // Start is called before the first frame update
        void Start()
        {
            // If the button is not set.
            if(button == null)
                button = GetComponent<Button>();

            // If the button rect is not set.
            if(buttonRect == null)
            {
                buttonRect = GetComponent<RectTransform>();
            }
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