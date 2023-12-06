using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // Triggers TTS upon the object being enabled.
    public class TextToSpeechOnEnable : MonoBehaviour
    {
        // The speak key.
        public string speakKey = "";

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            SpeakText();
        }

        // Speaks the text.
        public void SpeakText()
        {
            // Nothing set.
            if (speakKey == string.Empty)
                return;


            // The SDK is not initialized.
            if (!LOLManager.IsLOLSDKInitialized())
                return;

            // TTS is not enabled.
            if (!GameSettings.Instance.UseTextToSpeech)
                return;

            // Gets the instance.
            LOLManager lolManager = LOLManager.Instance;

            // Speak the text.
            lolManager.SpeakText(speakKey);
        }
    }
}