using System;
using System.Drawing;
using System.Numerics;

/// <summary> Global controller for updating world or general game state </summary>
public class GamespaceController : BaseController<GamespaceModel>
{
    public GamespaceController()
    {
        GameManager.Instance.TickUpdate += Update;    
    }

    private void Update()
    {
        PrimitivePlacementCheck();
    }

    private void PrimitivePlacementCheck()
    {
        if (!GameManager.TryGetTouchPosition(out var touchPosition))
            return;

        // Get raycast hit on plane object
        if (GameManager.ARRaycast(touchPosition, out var hitPoint))
        {
            var randomPrimitive = new Random().Next(0, 2);
            var primitive = randomPrimitive == 0 ? "Cube" : "Sphere";
            var uid = GameManager.CreatePrefab<PrimitiveObjectView, PrimitiveObjectController, PrimitiveObjectModel>(primitive);
            
            GameManager.SetObjectPosById(uid, hitPoint + new Vector3(0, 0.112f, 0));
            GameManager.SetObjectScaleById(uid, new Vector3(0.25f, 0.25f, 0.25f));
            
            var r = new Random().Next(0, 255);
            var g = new Random().Next(0, 255);
            var b = new Random().Next(0, 255);
            GameManager.SetObjectColorById(uid, r, g, b);
        }
    }
}