using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace RM_EM
{
    // A script attached to the plane used to render a puzzle.
    public class PuzzleRender : MonoBehaviour
    {
        // The manager.
        public MatchManager manager;

        // The render camera for the puzzle render object.
        public Camera renderCamera;

        public new BoxCollider collider;

        // Start is called before the first frame update
        void Start()
        {
            // If not set, grab collider.
            if(collider == null)
                collider = GetComponent<BoxCollider>();
        }

        // Calculates the hit given the world hit position on the puzzle render.
        public void CalculateHit(util.MouseButton mouseButton)
        {
            // CALCULATING THE HIT LOCATION ON THE OBJECT.
            // Grabs the hit point of the ray.
            Vector3 hitPoint = mouseButton.lastClickedHit.point;

            // Calculates the offset position from the render camera's center based on the render object's center.
            // Due to how the object is oriented, the object's (z) is used in place of (y) for this operation.
            Vector2 offsetPos = new Vector2();
            offsetPos.x = hitPoint.x - collider.bounds.center.x;
            offsetPos.y = hitPoint.y - collider.bounds.center.z;

            // Debug.Log(offsetPos.ToString());

            // TODO: check that the offset is calculating correctly. It seems to be?

            // CALCULATING THE RAY FROM THE RENDER CAMERA'S POSITION
            // Gets the ray's position for the render cmaera.
            Vector3 renderRayPos = renderCamera.transform.position + new Vector3(offsetPos.x, offsetPos.y, 0);

            // Copied from the mouse touch script.
            Vector3 target; // ray's target
            Ray ray; // ray object
            RaycastHit hitInfo; // info on hit.
            bool rayHit; // true if the ray hit.


            // Checks if the camera is perspective or orthographic.
            if (renderCamera.orthographic) // Orthographic
            {
                Debug.Log("Test");

                // Tries to get a hit. Since it's orthographic, the ray goes straight forward.
                target = renderCamera.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                // Ray position is render camera's position in world space, offset by the hit on the render plane.
                ray = new Ray(renderRayPos, target.normalized);

                // The max distance is the far clip plane minus the near clip plane.
                float maxDist = renderCamera.farClipPlane - renderCamera.nearClipPlane;

                // Cast the ray about as far as the camera can see.
                rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
            }
            else // Perspective
            {
                // TODO: FIX THIS. This needs to not use the main camera.

                // the target of the ray.
                target = util.MouseTouchInput.GetMouseTargetPositionInWorldSpace(renderCamera.gameObject);

                // ray object. It offsets so that objects not in the camera's clipping plane will be ignored.
                ray = new Ray(renderCamera.transform.position + renderCamera.transform.forward * renderCamera.nearClipPlane,
                    target.normalized);

                // the max distance is the far clip plane minus the near clip plane.
                float maxDist = renderCamera.farClipPlane - renderCamera.nearClipPlane;

                // the max distance
                rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
            }
            
            // The object that was hit by the replicated ray.
            GameObject hitObject = null;

            // If the ray hit an object successfully.
            if(rayHit)
            {
                hitObject = hitInfo.collider.gameObject;

                Debug.Log(hitObject.name);

                // TODO: do something with object (get value using script or something...)
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}