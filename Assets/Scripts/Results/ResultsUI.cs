using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The results UI.
    public class ResultsUI : MonoBehaviour
    {
        public ResultsManager manager;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = ResultsManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}