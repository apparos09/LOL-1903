using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

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

            // CALCULATING THE RAY FROM THE RENDER CAMERA'S POSITION
            // Gets the ray's position for the render cmaera.
            Vector3 renderRayPos = renderCamera.transform.position + new Vector3(offsetPos.x, offsetPos.y, 0);

            // // The render ray pos.
            // Debug.Log("Render Ray Pos: " + renderRayPos.ToString());

            // Copied from the mouse touch script.
            Vector3 target; // ray's target
            Ray ray; // ray object

            // Raycast Hits
            RaycastHit hitInfo; // info on hit (3D)
            RaycastHit2D hitInfo2D; // info on hit (2D)

            bool rayHit; // true if the ray hit.

            // The object that was hit by the replicated ray.
            GameObject hitObject = null;


            // 3D RAYCAST
            // Checks if the camera is perspective or orthographic.
            if (renderCamera.orthographic) // Orthographic
            {
                // Tries to get a hit. Since it's orthographic, the ray goes straight forward.
                target = renderCamera.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                // Ray position is render camera's position in world space, offset by the hit on the render plane.
                ray = new Ray(renderRayPos, target.normalized);

                // The max distance is the far clip plane minus the near clip plane.
                float maxDist = renderCamera.farClipPlane - renderCamera.nearClipPlane;

                // Cast the ray about as far as the camera can see.
                rayHit = Physics.Raycast(ray, out hitInfo, maxDist);

                // If the ray hit, mark this as NOT being a 2D hit (it was a 3D hit).
                if(rayHit)
                {
                    hitObject = hitInfo.collider.gameObject;
                }
            }
            else // Perspective
            {
                // I don't think it's even going into this function, but I'm leaving it here.
                // I'm pretty sure this doesn't work though.

                // Gets the render position in pixels.
                // TODO: this probably doesn't work. 
                Vector3 renderRayPosPixels = Camera.main.WorldToScreenPoint(renderRayPos);

                // The render camera's position in world space.
                Vector3 camWPos;

                // Calculates the world position based on the ray position.
                if (renderCamera.orthographic)
                    camWPos = renderCamera.ScreenToWorldPoint(new Vector3(renderRayPosPixels.x, renderRayPosPixels.y, renderCamera.nearClipPlane));
                else
                    camWPos = renderCamera.ScreenToWorldPoint(new Vector3(renderRayPosPixels.x, renderRayPosPixels.y, renderCamera.focalLength));

                // target = util.MouseTouchInput.GetMouseTargetPositionInWorldSpace(renderCamera.gameObject);
                target = camWPos - renderCamera.transform.position;

                // ray object. It offsets so that objects not in the camera's clipping plane will be ignored.
                ray = new Ray(renderCamera.transform.position + renderCamera.transform.forward * renderCamera.nearClipPlane,
                    target.normalized);

                // the max distance is the far clip plane minus the near clip plane.
                float maxDist = renderCamera.farClipPlane - renderCamera.nearClipPlane;

                // the max distance
                rayHit = Physics.Raycast(ray, out hitInfo, maxDist);

                // If the ray hit, get the game object from the collider.
                if(rayHit)
                {
                    hitObject = hitInfo.collider.gameObject;
                }
            }
            

            // 2D RAY CAST (ORTHOGRAPHIC ONLY)
            if(!rayHit)
            {
                // If the camera is orthographic, attempt a 2D raycast as well.
                if (Camera.main.orthographic)
                {
                    // setting up the 2D raycast for the orthographic camera.
                    RaycastHit2D rayHit2D = Physics2D.Raycast(
                        new Vector2(renderRayPos.x, renderRayPos.y),
                        new Vector2(target.normalized.x, target.normalized.y),
                        renderCamera.farClipPlane - renderCamera.nearClipPlane
                        );

                    // Save to hit info 2D.
                    hitInfo2D = rayHit2D;

                    // If a collider was hit, then the rayhit was successful.
                    rayHit = rayHit2D.collider != null;

                    // Checks rayHit value.
                    if (rayHit)
                    {
                        // Grab the hit object from the collider.
                        hitObject = rayHit2D.collider.gameObject;
                    }
                }
            }


            // If the ray hit an object successfully (hitObject should be set by this point)
            if(rayHit && hitObject != null)
            {
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