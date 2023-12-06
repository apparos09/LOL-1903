using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EM
{
    // A script for special functions for the tutorial text box.
    public class TutorialTextBox : MonoBehaviour
    {
        // The text box.
        public TextBox textBox;

        // The speaker image.
        public Image speakerImage;

        // The diagram image.
        public Image diagramImage;

        [Header("Diagram Sprites")]
        // The exponent diagram.
        public Sprite exponentDiagram;

        // The product diagram.
        public Sprite productDiagram;

        // The power of a power diagram.
        public Sprite powerOfAPowerDiagram;

        // The power of a product diagram.
        public Sprite powerOfAProductDiagram;

        // The zero expoent diagram.
        public Sprite zeroDiagram;

        // The negative diagram.
        public Sprite negativeDiagram;


        // SPEAKER //
        // Shows the speaker image.
        public void ShowSpeakerImage()
        {
            speakerImage.gameObject.SetActive(true);
        }

        // Hides the speaker iamge.
        public void HideSpeakerImage()
        {
            speakerImage.gameObject.SetActive(false);
        }

        // DIAGRAM //
        // Shows the diagram image.
        public void ShowDiagram(exponentRule rule)
        {
            // Shows the diagram.
            diagramImage.gameObject.SetActive(true);

            // Sets the diagram.
            switch(rule)
            {
                case exponentRule.exponent:
                    diagramImage.sprite = exponentDiagram;
                    break;

                case exponentRule.product:
                    diagramImage.sprite = productDiagram;
                    break;
                case exponentRule.powerOfAPower:
                    diagramImage.sprite = powerOfAPowerDiagram;
                    break;

                case exponentRule.powerOfAProduct:
                    diagramImage.sprite = powerOfAProductDiagram;
                    break;

                case exponentRule.zero:
                    diagramImage.sprite = zeroDiagram;
                    break;

                case exponentRule.negative:
                    diagramImage.sprite = negativeDiagram;
                    break;
            }

            // Hides the speaker image.
            HideSpeakerImage();
        }

        // Shows the exponent diagram.
        public void ShowExponentDiagram()
        {
            ShowDiagram(exponentRule.exponent);
        }
        
        // Shows the product diagram.
        public void ShowProductDiagram()
        {
            ShowDiagram(exponentRule.product);
        }

        // Shows the power of a power diagram.
        public void ShowPowerOfAPowerDiagram()
        {
            ShowDiagram(exponentRule.powerOfAPower);
        }

        // Shows the power of a product diagram.
        public void ShowPowerOfAProductDiagram()
        {
            ShowDiagram(exponentRule.powerOfAProduct);
        }

        // Shows the zero diagram.
        public void ShowZeroDiagram()
        {
            ShowDiagram(exponentRule.zero);
        }

        // Shows the negative diagam.
        public void ShowNegativeDiagram()
        {
            ShowDiagram(exponentRule.negative);
        }


        // Hides the diagram.
        public void HideDiagram()
        {
            diagramImage.sprite = null;
            diagramImage.gameObject.SetActive(false);

            // Shows the speaker if the diagam is hidden.
            ShowSpeakerImage();
        }
    }
}