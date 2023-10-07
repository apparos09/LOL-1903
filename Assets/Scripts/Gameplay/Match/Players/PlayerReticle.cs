using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // A reticle that's used to select values in th puzzle space.
    public class PlayerReticle : MonoBehaviour
    {
        // The player the reticle belongs to.
        public PlayerMatch playerMatch;

        // Start is called before the first frame update
        void Start()
        {

        }


        // TriggerEnter
        private void OnTriggerEnter(Collider other)
        {
            playerMatch.OnReticleTriggerEnter(other);
        }

        // TriggerEnter2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
            playerMatch.OnReticleTriggerEnter2D(collision);
        }

        // OnTriggerStay
        private void OnTriggerStay(Collider other)
        {
            playerMatch.OnReticleTriggerStay(other);
        }

        // OnTriggerStay2D
        private void OnTriggerStay2D(Collider2D collision)
        {
            playerMatch.OnReticleTriggerStay2D(collision);
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}