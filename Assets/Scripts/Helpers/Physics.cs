using System.Collections.Generic;
using UnityEngine;
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
    }
}