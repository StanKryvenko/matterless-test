using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Vector3 = System.Numerics.Vector3;

namespace Helpers
{
    public class Physics
    {
        public static void LookAt(Dictionary<int, (IView view, GameObject instance)> spawnedPrefabsMap, int id, Vector3 direction)
        {
            if (!spawnedPrefabsMap.TryGetValue(id, out var obj)) return;
            obj.instance.transform.LookAt(new UnityEngine.Vector3(direction.X, direction.Y, direction.Z));
        }

        public static Vector3 GetCameraPos()
        {
            var cameraPos = Camera.main.transform.position;
            return new Vector3(cameraPos.x, cameraPos.y, cameraPos.z);
        }

        public static bool ARRaycast(System.Numerics.Vector2 screenPoint, out Vector3 hitPoint, ARRaycastManager arRaycastManager)
        {
            var screenPoint3d = new Vector2(screenPoint.X, screenPoint.Y);
        
#if UNITY_EDITOR
            if (UnityEngine.Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint3d), out var hit))
            {
                hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                return true;
            }
#else
            var hits = new List<ARRaycastHit>();
            if (arRaycastManager != null && arRaycastManager.Raycast(screenPoint3d, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                hitPoint = new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z);
                return true;
            }
#endif
            hitPoint = Vector3.Zero;
            return false;
        }

        public static void SetObjectPosById(Dictionary<int, (IView view, GameObject instance)> spawnedPrefabsMap, int id, Vector3 position)
        {
            if (!spawnedPrefabsMap.TryGetValue(id, out var obj)) return;
            obj.instance.transform.position = new UnityEngine.Vector3(position.X, position.Y, position.Z);
        }

        public static void SetObjectColorById(Dictionary<int, (IView view, GameObject instance)> spawnedPrefabsMap, int id, Color color)
        {
            if (!spawnedPrefabsMap.TryGetValue(id, out var obj)) return;
            obj.instance.GetComponent<Renderer>().material.color = color;
        }

        public static void SetObjectScaleById(Dictionary<int, (IView view, GameObject instance)> spawnedPrefabsMap, int id, Vector3 scale)
        {
            if (!spawnedPrefabsMap.TryGetValue(id, out var obj)) return;
            obj.instance.transform.localScale = new UnityEngine.Vector3(scale.X, scale.Y, scale.Z);
        }
    }
}