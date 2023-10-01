using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The puzzle value script.
    public class PuzzleValue : MonoBehaviour
    {
        // The value saved to this puzzle entity.
        // 0-9, +, -, *, /
        public char value = '\0';

        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        // Called when the puzzle value is hit.
        // If 'destroy' is true, the value object is destroyed upon the value being returned.
        public char OnHit(bool destroy)
        {
            // Note: you should play some animation for removing the object.
            if(destroy)
                Destroy(gameObject, 0.5F);

            return value;
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}