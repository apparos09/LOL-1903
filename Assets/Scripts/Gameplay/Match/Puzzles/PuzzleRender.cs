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

        // The puzzle this render if for.
        public Puzzle puzzle;

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
        // Returns the hit object.
        public GameObject TryHit(util.MouseButton mouseButton)
        {
            // CALCULATING THE HIT LOCATION ON THE OBJECT.
            // Grabs the hit point of the ray (impact point is in world space).
            Vector3 hitPoint = mouseButton.lastClickedHit.point;

            // The bounds information is in world space.
            // The hit point in 0-1 space.
            Vector3 hitPoint01 = new Vector3();

            // Calculates the hit point in a 0-1 space by checking the bounds of the collider (in world space).
            hitPoint01.x = Mathf.InverseLerp(collider.bounds.min.x, collider.bounds.max.x, hitPoint.x);
            hitPoint01.y = Mathf.InverseLerp(collider.bounds.min.y, collider.bounds.max.y, hitPoint.y);
            hitPoint01.z = Mathf.InverseLerp(collider.bounds.min.z, collider.bounds.max.z, hitPoint.z);

            //// New - Camera Scale
            //// Modifies the scale of the offset so that matches up with the camera.
            //Vector3 camScale = new Vector3(0, 0, 1);

            //// TODO: change this to use a percentage of the quad size and camera view.

            //// Checks if the camera is orthographic or perspective.
            //if(renderCamera.orthographic) // Orthographic
            //{
            //    // Camera Size / Collider Size

            //    // Ver. 1
            //    // camScale.x = renderCamera.orthographicSize / collider.size.x;
            //    // camScale.y = renderCamera.orthographicSize / collider.size.y;

            //    // Ver. 2
            //    camScale.x = renderCamera.pixelWidth / collider.size.x;
            //    camScale.y = renderCamera.pixelHeight / collider.size.y;

            //}
            //else // Perspective
            //{
            //    // Checks the camera's size in pixels so that it can convert the collider size to it.
            //    // I don't know if this works, but I probably won't use it anyway.
            //    // (Camera Pixel Size) / Collider Size
            //    camScale.x = renderCamera.pixelWidth / collider.size.x;
            //    camScale.y = renderCamera.pixelHeight / collider.size.y;
            //}


            // Calculates the offset position from the render camera's center based on the render object's center.
            // Due to how the object is oriented, the object's (z) is used in place of (y) for this operation.
            Vector2 offsetPos = new Vector2();
            offsetPos.x = hitPoint.x - collider.bounds.center.x;
            offsetPos.y = hitPoint.y - collider.bounds.center.z;

            // CALCULATING THE RAY FROM THE RENDER CAMERA'S POSITION
            // Gets the ray's position for the render camera.

            // Old
            // Using the old version for now so I can finish the pinball.
            Vector3 renderRayPos = renderCamera.transform.position + new Vector3(offsetPos.x, offsetPos.y, 0);

            // New
            // Vector3 renderRayPos = renderCamera.transform.position + Vector3.Scale(new Vector3(offsetPos.x, offsetPos.y, 0), camScale);

            // New

            Debug.Log("Hit Point 01: " + hitPoint01.ToString());
            
            // Converts the hit position on the render to a point in the camera in world space.
            // The hitpoint is in 0-1 space in reference the bounds of the collider. As such, the viewport conversion...
            // Has it match up with the bounds of the camera.
            renderRayPos = renderCamera.ViewportToWorldPoint(hitPoint01);
            renderRayPos.z = renderCamera.transform.position.z;


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

                // TODO: why do I check for orthographic twice? Take that out.
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
                // TODO: comment out.
                Debug.Log(hitObject.name);
            }

            // Returns the hit object.
            return hitObject;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}