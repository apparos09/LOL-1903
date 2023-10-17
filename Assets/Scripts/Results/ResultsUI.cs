using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_EM
{
    // The results UI.
    public class ResultsUI : MonoBehaviour
    {
        public ResultsManager manager;

        // The game time text.
        public TMP_Text gameTimeText;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = ResultsManager.Instance;
        }

        // Applies the results data.
        public void ApplyResultsData(ResultsData data)
        {
            gameTimeText.text = data.gameTime.ToString("F2");

            // TODO: add more.
        }
    }
}