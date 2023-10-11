using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EM
{
    // A general tutorial class.
    public abstract class Tutorial : MonoBehaviour
    {
        // // The game manager.
        // public GameplayManager gameManager;

        // The text box for the tutorial.
        public TextBox textBox;


        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If the textbox is visible, close it.
            if(textBox.IsVisible())
                textBox.Close();


            // Maybe do something like this?
            // textBox.OnTextBoxOpenedAddCallback(gameManager.OnTutorialStart);
            // textBox.OnTextBoxClosedAddCallback(gameManager.OnTutorialEnd);
        }

        // Sets the pages to the text box.
        // If 'activate' is true, the textbox is also activated.
        public void SetPagesToTextbox(List<Page> newPages, bool openTextBox)
        {
            textBox.pages = newPages;
            textBox.CurrentPageIndex = 0;
            
            // Opens the textbox.
            if(openTextBox)
            {
                textBox.Open();
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

        // Update is called once per frame
        protected virtual void Update()
        {
            // ...
        }
    }
}