using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The world camera.
    public class WorldCamera : MonoBehaviour
    {
        // The camera script.
        public new Camera camera;

        [Header("Transition")]
        // The camera's destination position.
        public Vector3 destPos;

        // Sets set to 'true', when the camera is transitioning to another position.
        public bool inTransition = false;

        // The camera's movement speed.
        public float speed = 10.0F;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, try to get the camera component.
            if(camera == null)
            {
                // If that doesn't work, set it as the main camera.
                if(!TryGetComponent<Camera>(out camera))
                {
                    camera = Camera.main;
                }
            }

            // Set to camera's position by default.
            destPos = transform.position;
        }

        // Sets the destination position.
        public void Move(Vector3 newPos)
        {
            destPos = newPos;
            inTransition = true;
        }

        // Sets the destination position using an object.
        public void Move(GameObject dest)
        {
            Move(dest.transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            // Checks if the camera is transitioning.
            if(inTransition)
            {
                // TODO: replace with easing lerp?
                
                // Generate the new position.
                Vector3 newPos = Vector3.MoveTowards(transform.position, destPos, speed * Time.deltaTime);

                // Set the new position.
                transform.position = newPos;

                // Destination reached.
                if(newPos == destPos)
                {
                    inTransition = false;
                }
            }
        }
    }
}