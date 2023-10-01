using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace util
{
    // Dynamically scales TMP text so that it fits in the bounds.
    public class TextScaler : MonoBehaviour
    {
        // NOTE: you shouldn't have 2 forms of text here, but I didn't want to make seperate files.
        // TMP_Text has a parameter with hasPropertiesChanged, which regular text doesn't have. Maybe use that...
        // If the files are split into 2 later.

        // Regular text.
        public Text text;

        // TMP text.
        public TMP_Text tmpText;

        // If set to 'true', text wrapping is automatically disabled.
        public bool disableWrappingOnStart = true;

        // The maximum amount of characters before dynamic scaling is applied.
        public int charLimit = -1;

        [Header("Scaling")]
        // The base text scale.
        public Vector3 baseScale = Vector3.one;

        // The base TMP text scale.
        public Vector3 baseScaleTmp = Vector3.one;

        // Determines what to scale.
        public bool scaleX = true;
        public bool scaleY = false;
        public bool scaleZ = false;

        // If set to 'true', the starting scale of the text is used to set the baseScale.
        public bool autoSetBaseScale = true;

        // Auto sets the base scale for TMP text.
        public bool autoSetBaseScaleTmp = true;

        [Header("Other")]
        
        // If set to 'true', the text scaler is automatically updated.
        public bool autoUpdate = true;

        // Start is called before the first frame update
        void Start()
        {
            // Autoset the text.
            if (text == null)
                text = GetComponent<Text>();

            // Autoset the tmp text.
            if (tmpText == null)
                tmpText = GetComponent<TMP_Text>();


            // If text wrapping should be automatically disabled. This only applies to TMP text.
            if(disableWrappingOnStart && tmpText != null)
                tmpText.enableWordWrapping = false;

            // Auto sets the base scale.
            if (autoSetBaseScale && text != null)
                baseScale = text.transform.localScale;

            // Auto sets the base scale for TMP text.
            if (autoSetBaseScaleTmp && tmpText != null)
                baseScaleTmp = tmpText.transform.localScale;
        }

        // Scales the text when the script is enabled.
        private void OnEnable()
        {
            SetTextScale();
        }

        // Sets the text scale.
        public void SetTextScale()
        {
            // Set scale for regular text.
            if(text != null)
            {
                // If the text length has exceeded the character limit.
                if (text.text.Length > charLimit)
                {
                    // Gets the scale factor.
                    float factor = (float)charLimit / text.text.Length;

                    // The new scale.
                    Vector3 newScale = baseScale;

                    // Scale by x-factor.
                    if (scaleX)
                        newScale.x *= factor;

                    // Scale the y-factor.
                    if (scaleY)
                        newScale.y *= factor;

                    // Scale the z-factor.
                    if (scaleZ)
                        newScale.z *= factor;

                    // Set the text scale.
                    text.transform.localScale = newScale;
                }
                else
                {
                    // If the text scale is not set to its base, set it back to normal.
                    if (text.transform.localScale != baseScale)
                    {
                        // Set to the default.
                        text.transform.localScale = baseScale;
                    }
                }
            }

            // Set scale for TMP text.
            if (tmpText != null)
            {
                // If the text length has exceeded the character limit.
                if (tmpText.text.Length > charLimit)
                {
                    // Gets the scale factor.
                    float factor = (float)charLimit / tmpText.text.Length;

                    // The new scale.
                    Vector3 newScale = baseScaleTmp;

                    // Scale by x-factor.
                    if (scaleX)
                        newScale.x *= factor;

                    // Scale the y-factor.
                    if (scaleY)
                        newScale.y *= factor;

                    // Scale the z-factor.
                    if (scaleZ)
                        newScale.z *= factor;

                    // Set the text scale.
                    tmpText.transform.localScale = newScale;
                }
                else
                {
                    // If the text scale is not set to its base, set it back to normal.
                    if (tmpText.transform.localScale != baseScale)
                    {
                        // Set to the default.
                        tmpText.transform.localScale = baseScale;
                    }
                }
            }

        }

        // Update is called once per frame
        void Update()
        {
            // If text should be auto-updated.
            if (autoUpdate)
                SetTextScale();
        }
    }
}